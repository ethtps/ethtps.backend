using ETHTPS.Data.Core;

using Hangfire;
using Hangfire.InMemory;
using Hangfire.SqlServer;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.API.DependencyInjection
{
    public static class HangfireExtensions
    {
        private const string _DEFAULT_CONNECTION_STRING_NAME = "HangfireConnectionString";
        private static InMemoryStorage _storage = new();
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
                JobStorage.Current = _storage;
                services.AddHangfire(config =>
                {
                    config.UseInMemoryStorage();
                });
            }
            else
            {
                services.InitializeHangfireWithDBStorage(microservice);
            }
            return services;
        }

        public static IApplicationBuilder UseHangfire(this IApplicationBuilder app, string[] configurationQueues) // Called in Configure(...)
        {
#pragma warning disable CS0618 
            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                Queues = configurationQueues,
                SchedulePollingInterval = TimeSpan.FromSeconds(2)
            }, storage: _storage);
#pragma warning restore CS0618 
            app.UseHangfireDashboard();
            return app;
        }
    }
}