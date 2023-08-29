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
        public static void InitializeHangfire(this IServiceCollection services, ETHTPSMicroservice microservice)
        {
            SqlServerStorage sqlStorage = new(services.GetConnectionString(microservice, _DEFAULT_CONNECTION_STRING_NAME));
            JobStorage.Current = sqlStorage;
        }

        public static IServiceCollection AddHangfireServer(this IServiceCollection services, ETHTPSMicroservice microservice, bool inMemoryStorage = true)
        {
            Hangfire.JobStorage.Current = inMemoryStorage ? new InMemoryStorage() : new SqlServerStorage(services.GetConnectionString(microservice, _DEFAULT_CONNECTION_STRING_NAME));
            if (!inMemoryStorage)
                services.AddHangfire(x => x.UseSqlServerStorage(services.GetConnectionString(microservice, _DEFAULT_CONNECTION_STRING_NAME)));
            else services.AddHangfire(x => x.UseInMemoryStorage());
            services.AddHangfireServer(options =>
            {
                options.SchedulePollingInterval = TimeSpan.FromSeconds(5);
            });
            return services;
        }

        public static IApplicationBuilder ConfigureHangfire(this IApplicationBuilder app, string[] configurationQueues)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            app.UseHangfireServer(options: new BackgroundJobServerOptions()
            {
                Queues = configurationQueues
            });
#pragma warning restore CS0618 // Type or member is obsolete
            return app;
        }
    }
}