using Hangfire;
using Hangfire.SqlServer;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.API.DependencyInjection
{
    public static class HangfireExtensions
    {
        private const string _DEFAULT_CONNECTION_STRING_NAME = "HangfireConnectionString";
        public static void InitializeHangfire(this IServiceCollection services, string appName)
        {
            SqlServerStorage sqlStorage = new(services.GetConnectionString(appName, _DEFAULT_CONNECTION_STRING_NAME));
            JobStorage.Current = sqlStorage;
        }

        public static IServiceCollection AddHangfireServer(this IServiceCollection services, string appName)
        {
            Hangfire.JobStorage.Current = new SqlServerStorage(services.GetConnectionString(appName, _DEFAULT_CONNECTION_STRING_NAME));
            services.AddHangfire(x => x.UseSqlServerStorage(services.GetConnectionString(appName, _DEFAULT_CONNECTION_STRING_NAME)));
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