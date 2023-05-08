using System.Reflection;

using Coravel;
using Coravel.Scheduling.Schedule.Interfaces;

using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.Core.Integrations.MSSQL.Services.TimeBuckets.Extensions;
using ETHTPS.API.Core.Integrations.MSSQL.Services.Updater;
using ETHTPS.Configuration;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Services;
using ETHTPS.Services.Attributes;
using ETHTPS.Services.BlockchainServices.BlockTime;
using ETHTPS.Services.BlockchainServices.CoravelLoggers;
using ETHTPS.Services.BlockchainServices.HangfireLogging;
using ETHTPS.Services.Ethereum;
using ETHTPS.Services.Ethereum.JSONRPC.Infura;
using ETHTPS.Services.Ethereum.Scan.Implementations;
using ETHTPS.Services.Ethereum.Starkware;

using Hangfire;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.API.DependencyInjection
{
    public static class DataUpdaterExtensions
    {
        public static IServiceCollection AddDataUpdaterStatusService(this IServiceCollection services) =>
            services.AddTransient<IDataUpdaterStatusService, DataUpdaterStatusService>();

        private static Type[] _enabledUpdaters = new[]
        {
            typeof(EthereumBlockInfoProvider),
            typeof(AuroraBlockInfoProvider),
            typeof(AVAXBlockInfoProvider),
            typeof(CeloBlockInfoProvider),
            typeof(NEARBlockInfoProvider),
            typeof(PalmBlockInfoProvider),
            typeof(PolygonBlockInfoProvider),
            typeof(StarknetBlockInfoProvider),
            typeof(PolygonBlockInfoProvider),
            typeof(ArbitrumBlockInfoProvider),
            typeof(OptimismBlockInfoProvider),
            typeof(LoopringBlockInfoProvider),
            typeof(BobaNetworkBlockInfoProvider),
            typeof(XDAIHTTPBlockInfoProvider),
            typeof(ZKSwapBlockInfoProvider),
            typeof(ZKSpaceBlockInfoProvider),
            typeof(ZKSsyncBlockInfoProvider),
            typeof(AztecBlockInfoProvider),
            typeof(ImmutableXBlockInfoProvider),
            typeof(MetisBlockInfoProvider),
            typeof(RoninBlockInfoProvider),
            typeof(VoyagerBlockInfoProvider),
            typeof(Nahmii20BlockInfoProvider),
            typeof(BSCScanBlockInfoProvider),
            typeof(OMGNetworkBlockInfoProvider),
            typeof(ZKTubeBlockInfoProvider),
            typeof(FTMScanBlockInfoProvider),
            typeof(SorareBlockInfoProvider),
            typeof(DeversiFIHTTPBlockInfoProvider),
            typeof(PolygonHermezBlockInfoProvider),
        };
        public static IServiceCollection AddDataServices(this IServiceCollection services) => services.AddScoped(_enabledUpdaters);
        public static IServiceCollection AddRunner(this IServiceCollection services, BackgroundServiceType type)
        {
            services.AddScoped<EthereumBlockTimeProvider>();
            switch (type)
            {
                case BackgroundServiceType.Coravel:
                    services.AddScheduler();
                    services.AddScoped(_enabledUpdaters.Select(x => typeof(CoravelBlockLogger<>).MakeGenericType(x)));
                    break;
                case BackgroundServiceType.Hangfire:
                    services.AddHangfireServer("ETHTPS.TaskRunner");
                    //_enabledUpdaters.ToList().ForEach(updater => services.RegisterHangfireBackgroundService(updater));
                    services.RegisterHangfireBackgroundServiceAndTimeBucket<MSSQLLogger<EthereumBlockInfoProvider>, EthereumBlockInfoProvider>(CronConstants.EVERY_5_S, "tpsdata");


                    break;
            }
            return services;
        }

        public static void UseRunner(this IApplicationBuilder app, BackgroundServiceType type)
        {
            switch (type)
            {
                case BackgroundServiceType.Coravel:
                    app.UseCoravel();
                    break;
                case BackgroundServiceType.Hangfire:
                    using (var scope = app.ApplicationServices.CreateScope())
                    {
                        var provider = scope.ServiceProvider.GetRequiredService<IDBConfigurationProvider>();
                        string[] queues = provider.GetConfigurationStrings("HangfireQueue").Select(x => x.Value).ToArray();
                        if (queues.Count() == 0)
                        {
                            queues = new string[] { "default" };
                        }
                        app.ConfigureHangfire(queues);
                        app.UseHangfireDashboard();
                        break;
                    }
            }
        }

        public static IServiceCollection WithStore(this IServiceCollection services, DatabaseProvider databaseProvider, string appName)
        {
            switch (databaseProvider)
            {
                case DatabaseProvider.MSSQL:
                    services.InitializeHangfire(appName);
                    break;
            }
            return services;
        }

        public static void RegisterHangfireBackgroundService(this IServiceCollection services, Type sourceType)
        {
            if (!typeof(IHTTPBlockInfoProvider).IsAssignableFrom(sourceType))
            {
                throw new ArgumentException("The provided type must implement IHTTPBlockInfoProvider.", nameof(sourceType));
            }
            var runsEveryAttribute = sourceType.GetCustomAttribute<RunsEveryAttribute>();
            var cronExpression = runsEveryAttribute?.CronExpression ?? CronConstants.EVERY_MINUTE;
            var genericMethod = typeof(DataUpdaterExtensions).GetMethod(nameof(RegisterHangfireBackgroundServiceAndTimeBucket));
            var loggerType = typeof(MSSQLLogger<>).MakeGenericType(sourceType);
            var constructedMethod = genericMethod?.MakeGenericMethod(loggerType, sourceType);
            constructedMethod?.Invoke(null, new object[] { services, cronExpression, "tpsdata" });
        }

        public static void RegisterHangfireBackgroundServiceAndTimeBucket<T, V>(this IServiceCollection services, string cronExpression, string queue)
            where T : MSSQLLogger<V>
            where V : class, IHTTPBlockInfoProvider
        {
            services.AddScoped<V>();
            services.InjectTimeBucketService<V>();
            services.AddScoped<T>();
#pragma warning disable CS0618 // Type or member is obsolete
            Hangfire.RecurringJob.AddOrUpdate<T>(typeof(V).Name, x => x.RunAsync(), cronExpression, queue: queue);
#pragma warning restore CS0618 // Type or member is obsolete
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
                            IScheduleInterval? interval = (IScheduleInterval?)generic.Invoke(scheduler, null);
                            if (interval == null)
                                throw new Exception("Initialization failed");
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
                            Console.WriteLine($"Registered {loggerType.Name}<{loggerType.GetGenericArguments()[0].Name}>");
                        }
                    });
                }
                else throw new Exception("Couldn't find method");
            });
        }
    }
}
