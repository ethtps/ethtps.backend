using ETHTPS.Data.Core;

using Hangfire;
using Hangfire.SqlServer;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.API.DependencyInjection
{
    public static class HangfireExtensions
    {
        private const string _DEFAULT_CONNECTION_STRING_NAME = "HangfireConnectionString";
        private static void InitializeHangfireWithDBStorage(this IServiceCollection services, Microservice microservice)
        {
            SqlServerStorage sqlStorage = new(services.GetConnectionString(microservice, _DEFAULT_CONNECTION_STRING_NAME));
            JobStorage.Current = sqlStorage;
            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(services.GetConnectionString(microservice, _DEFAULT_CONNECTION_STRING_NAME));
            });
        }

        public static IServiceCollection AddHangfireServer(this IServiceCollection services, Microservice microservice, bool inMemoryStorage = true) // Called in ConfigureServices(...)
        {
            if (inMemoryStorage)
            {
                GlobalConfiguration.Configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                           .UseColouredConsoleLogProvider()
                           .UseSimpleAssemblyNameTypeSerializer()
                           .UseRecommendedSerializerSettings()
                           .UseInMemoryStorage();
                services.AddHangfire(config =>
                {
                    /*   config.UseInMemoryStorage(new Hangfire.InMemory.InMemoryStorageOptions()
                       {
                           StringComparer = System.StringComparer.InvariantCultureIgnoreCase,
                           DisableJobSerialization = true
                       });*/
                });
            }
            else
            {
                services.InitializeHangfireWithDBStorage(microservice);
            }
            services.AddHangfireServer(options =>
            {
                options.SchedulePollingInterval = TimeSpan.FromSeconds(2);
                options.IsLightweightServer = false;
            });
            return services;
        }

        public static IApplicationBuilder UseHangfire(this IApplicationBuilder app, string[] configurationQueues)
        {
            app.UseHangfireDashboard();
            return app;
        }
    }
}