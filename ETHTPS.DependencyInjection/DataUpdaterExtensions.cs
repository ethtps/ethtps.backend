using System.Reflection;

using Coravel;
using Coravel.Scheduling.Schedule.Interfaces;

using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.Core.Integrations.MSSQL.Services.TimeBuckets.Extensions;
using ETHTPS.API.Core.Integrations.MSSQL.Services.Updater;
using ETHTPS.Configuration;
using ETHTPS.Configuration.Validation.Exceptions;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Services;
using ETHTPS.Services.BlockchainServices;
using ETHTPS.Services.BlockchainServices.BlockTime;
using ETHTPS.Services.BlockchainServices.CoravelLoggers;
using ETHTPS.Services.BlockchainServices.HangfireLogging;
using ETHTPS.Services.Ethereum;
using ETHTPS.Services.Ethereum.JSONRPC.Implementations;
using ETHTPS.Services.Ethereum.JSONRPC.Infura;
using ETHTPS.Services.Ethereum.Scan.Implementations;
using ETHTPS.Services.Ethereum.Starkware;

using Hangfire;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using static ETHTPS.Utils.Logging.LoggingUtils;

namespace ETHTPS.API.DependencyInjection
{
    public static class DataUpdaterExtensions
    {
        public static IServiceCollection AddDataUpdaterStatusService(this IServiceCollection services) =>
            services.AddTransient<IDataUpdaterStatusService, DataUpdaterStatusService>();

        private static Type[] _enabledUpdaters = new[]
        {
            typeof(EthereumBlockInfoProvider),
            typeof(AuroraJSONRPCBlockInfoProvider),
            typeof(AVAXBlockInfoProvider),
            typeof(CeloBlockInfoProvider),
            typeof(NEARBlockInfoProvider),
            typeof(PalmBlockInfoProvider),
            typeof(Services.Ethereum.JSONRPC.Infura.PolygonBlockInfoProvider),
            typeof(StarknetBlockInfoProvider),
            typeof(ArbitrumBlockInfoProvider),
            typeof(OptimismBlockInfoProvider),
            typeof(LoopringBlockInfoProvider),
            typeof(BobaNetworkBlockInfoProvider),
            typeof(XDAIHTTPBlockInfoProvider),
            typeof(ZKSwapBlockInfoProvider),
            typeof(ZKSpaceBlockInfoProvider),
            typeof(ZKSsyncBlockInfoProvider),
            typeof(ZKSsyncEraBlockInfoProvider),
            typeof(AztecBlockInfoProvider),
            typeof(ImmutableXBlockInfoProvider),
            typeof(MetisBlockInfoProvider),
            typeof(RoninBlockInfoProvider),
            typeof(VoyagerBlockInfoProvider),
            typeof(Nahmii20BlockInfoProvider),
            typeof(OMGNetworkBlockInfoProvider),
            typeof(ZKTubeBlockInfoProvider),
            typeof(FTMScanBlockInfoProvider),
            typeof(SorareBlockInfoProvider),
            typeof(DeversiFIHTTPBlockInfoProvider),
            typeof(PolygonHermezBlockInfoProvider),
            typeof(GnosisJSONRPCBlockInfoProvider)
        };

        /// <summary>
        /// Manually registers cherry-picked services to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataServices(this IServiceCollection services) => services.AddScoped(_enabledUpdaters);

        public static IServiceCollection AddRunner(this IServiceCollection services, BackgroundServiceType type, Microservice microservice, DatabaseProvider runnerDatabaseProvider)
        {
            services.AddScoped<EthereumBlockTimeProvider>();
            switch (type)
            {
                case BackgroundServiceType.Coravel:
                    services.AddScheduler();
                    services.AddScoped(_enabledUpdaters.Select(x => typeof(CoravelBlockLogger<>).MakeGenericType(x)));
                    break;
                default:
                    services.AddHangfireServer(microservice, runnerDatabaseProvider == DatabaseProvider.InMemory);
                    _enabledUpdaters.ToList().ForEach(updater => services.RegisterHangfireBackgroundService(updater, runnerDatabaseProvider));
                    services.InjectTimeBucketService(runnerDatabaseProvider);
                    break;
            }
            return services;
        }

        /// <summary>
        /// Starts the task runner.
        /// </summary>
        /// <param name="app">Who is requesting this?</param>
        /// <param name="type">What type of runner are we using again?</param>
        /// <exception cref="ConfigurationStringNotFoundException"></exception>
        public static void UseTaskRunner(this IApplicationBuilder app, BackgroundServiceType type)
        {
            switch (type)
            {
                case BackgroundServiceType.Coravel:
                    app.UseCoravel();
                    break;
                case BackgroundServiceType.Hangfire:
                    using (var scope = app.ApplicationServices.CreateScope())
                    {
                        var provider = scope.ServiceProvider.GetRequiredService<DBConfigurationProviderWithCache>();
                        string[] queues = (provider.GetConfigurationStrings("HangfireQueue")
                            ?? throw new ConfigurationStringNotFoundException("HangfireQueue", "UseRunner(this IApplicationBuilder app, BackgroundServiceType type)"))
                            .Select(x => x.Value)
                            .ToArray();
                        if (queues.Length == 0)
                        {
                            queues = new string[] { "default" };
                        }
                        app.UseHangfire(queues);
                        break;
                    }
            }
        }

        public static IServiceCollection WithStore(this IServiceCollection services, DatabaseProvider databaseProvider, Microservice microservice) => services.AddHangfireServer(microservice, databaseProvider != DatabaseProvider.MSSQL);

        public static void RegisterHangfireBackgroundService(this IServiceCollection services, Type sourceType, DatabaseProvider runnerDatabaseProvider)
        {
            if (!typeof(IHTTPBlockInfoProvider).IsAssignableFrom(sourceType))
            {
                throw new ArgumentException("The provided type must implement IHTTPBlockInfoProvider.", nameof(sourceType));
            }
            var runsEveryAttribute = sourceType.GetCustomAttribute<RunsEveryAttribute>();
            var cronExpression = runsEveryAttribute?.CronExpression ?? CronConstants.EVERY_MINUTE;
            var genericMethod = typeof(DataUpdaterExtensions).GetMethod(nameof(RegisterHangfireBackgroundServiceAndTimeBucket));
            var loggerType = (runnerDatabaseProvider == DatabaseProvider.MSSQL ? throw new InvalidOperationException("Not supported anymore") : typeof(InfluxLogger<>)).MakeGenericType(sourceType);
            var constructedMethod = genericMethod?.MakeGenericMethod(loggerType, sourceType);
            constructedMethod?.Invoke(null, new object[] { services, cronExpression, "tpsdata" });
        }

        public static void RegisterHangfireBackgroundServiceAndTimeBucket<T, V>(this IServiceCollection services, string cronExpression, string queue)
            where T : BlockInfoProviderDataLoggerBase<V>
            where V : class, IHTTPBlockInfoProvider
        {
            services.AddScoped<V>();
            services.AddScoped<T>();
            RecurringJob.AddOrUpdate<T>(typeof(V).Name, queue, x => x.RunAsync(), cronExpression, new RecurringJobOptions()
            {
                MisfireHandling = MisfireHandlingMode.Ignorable
            });
            Trace($"Registered {typeof(T).Name.Replace("`1", "")}<{typeof(V).Name}> [{queue}:{cronExpression}]");
        }

        private static void UseCoravel(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices;
            provider.UseScheduler(scheduler =>
            {
                var blockLoggerTypes = _enabledUpdaters.Select(x => typeof(CoravelBlockLogger<>).MakeGenericType(x));

                var methods = typeof(IScheduler).GetMethods();
                var method = methods.Where(x => x.Name == nameof(IScheduler.Schedule)).FirstOrDefault(x => x.IsGenericMethod);
                if (method != null)
                {
                    blockLoggerTypes.ToList().ForEach(loggerType =>
                    {
                        var attributes = loggerType.GetCustomAttributes(true);
                        if (!attributes.Any(x => x.GetType() == typeof(DisabledAttribute)))
                        {
                            var generic = method.MakeGenericMethod(loggerType);
                            IScheduleInterval? interval = (IScheduleInterval?)generic.Invoke(scheduler, null) ?? throw new Exception("Initialization failed");
                            if (attributes.Any(x => x.GetType() == typeof(RunsEveryAttribute)))
                            {
                                var runsEvery = loggerType.GetCustomAttribute<RunsEveryAttribute>();
                                if (runsEvery != null)
                                {
                                    interval?.Cron(runsEvery.CronExpression);
                                }
                            }
                            else
                            {
                                interval?.EveryFifteenSeconds();
                            }
                            Trace($"Registered {loggerType.Name}<{loggerType.GetGenericArguments()[0].Name}>");
                        }
                    });
                }
                else throw new Exception("Couldn't find method");
            });
        }
    }
}
