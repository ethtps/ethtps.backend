using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Services.BlockchainServices.HangfireLogging;
using ETHTPS.Services.BlockchainServices.Status;

using Hangfire;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using static ETHTPS.Utils.Logging.LoggingUtils;

namespace ETHTPS.Services.Infrastructure.Extensions
{
    public static class BackgroundServiceExtensions
    {
#pragma warning disable CS0618

        public static void RegisterInfluxHangfireBackgroundService<T, V>(this IServiceCollection services, string cronExpression, string queue)
           where V : class, IHTTPBlockInfoProvider
           where T : InfluxLogger<V>
        {
            services.AddScoped<V>();
            services.AddScoped<T>();
            RecurringJob.AddOrUpdate<T>(typeof(V).Name, x => x.RunAsync(), cronExpression, queue: queue);
            Trace($"Registered ${typeof(T).Name} into ${queue}");
        }

        public static void RegisterInfluxHangfireHistoricalBackgroundService<T, V>(this IServiceCollection services)
           where V : class, IHTTPBlockInfoProvider
           where T : HistoricalInfluxLogger<V>
        {
            services.TryAddScoped<V>();
            services.AddScoped<T>();
            BackgroundJob.Enqueue<T>(x => x.RunAsync());
            Trace($"Registered ${typeof(T).Name}<${typeof(V).Name}>");
        }

        public static void RegisterHangfireBackgroundService<T>(this IServiceCollection services, string cronExpression, string queue)
            where T : HangfireBackgroundService
        {
            services.AddScoped<T>();
            RecurringJob.AddOrUpdate<T>(typeof(T).Name, x => x.RunAsync(), cronExpression, queue: queue);
            Trace($"Registered ${typeof(T).Name} into ${queue}");
        }

#pragma warning restore CS0618
    }
}
