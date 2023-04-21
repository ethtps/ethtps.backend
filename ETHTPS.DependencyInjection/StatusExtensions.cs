using ETHTPS.Services.BlockchainServices.Status.BackgroundTasks.Discord.Endpoints;
using ETHTPS.Services.BlockchainServices.Status.BackgroundTasks.Discord;
using ETHTPS.Services;
using Microsoft.Extensions.DependencyInjection;
using static ETHTPS.API.Core.Constants;
using ETHTPS.Services.Infrastructure.Extensions;

namespace ETHTPS.API.DependencyInjection
{
    public static class StatusExtensions
    {
        public static IServiceCollection AddStatusNotifiers(this IServiceCollection services, string[] configurationQueues)
        {
            if (configurationQueues.Contains(STATUSUPDATERQUEUE))
            {
                services.RegisterHangfireBackgroundService<MainAPIStatusBackgroundTask>(CronConstants.EVERY_MINUTE, STATUSUPDATERQUEUE);
                services.RegisterHangfireBackgroundService<TPSAPIStatusBackgroundTask>(CronConstants.EVERY_MINUTE, STATUSUPDATERQUEUE);
                services.RegisterHangfireBackgroundService<GPSAPIStatusBackgroundTask>(CronConstants.EVERY_MINUTE, STATUSUPDATERQUEUE);
                services.RegisterHangfireBackgroundService<GeneralAPIStatusBackgroundTask>(CronConstants.EVERY_MINUTE, STATUSUPDATERQUEUE);
                services.RegisterHangfireBackgroundService<TimeWarpAPIStatusBackgroundTask>(CronConstants.EVERY_MINUTE, STATUSUPDATERQUEUE);
                services.RegisterHangfireBackgroundService<WebsiteStatusBackgroundTask>(CronConstants.EVERY_MINUTE, STATUSUPDATERQUEUE);
                //services.RegisterHangfireBackgroundService<UpdaterStatusBackgroundTask>(CronConstants.EveryMinute, STATUSUPDATERQUEUE);
                services.RegisterHangfireBackgroundService<PlausibleVisitorCountBackgroundTask>(CronConstants.EVERY_MIDNIGHT, STATUSUPDATERQUEUE);
            }
            return services;
        }
    }
}
