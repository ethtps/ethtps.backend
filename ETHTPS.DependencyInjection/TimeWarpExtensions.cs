using ETHTPS.Services;
using ETHTPS.Services.BlockchainServices.HangfireLogging;
using ETHTPS.Services.Ethereum.JSONRPC.Infura;
using ETHTPS.Services.Infrastructure.Extensions;

using Microsoft.Extensions.DependencyInjection;

using static ETHTPS.API.Core.Constants;

namespace ETHTPS.API.DependencyInjection
{
    public static class TimeWarpExtensions
    {
        public static IServiceCollection AddTimeWarpUpdaters(this IServiceCollection services, string[] configurationQueues)
        {
            if (configurationQueues.Contains(TIMEWARPUPDATERQUEUE))
            {
                services.RegisterTimeWarpHangfireBackgroundService<TimeWarpBlockInfoProviderDataLogger<InfuraBlockInfoProviderBase>, InfuraBlockInfoProviderBase>(CronConstants.NEVER, TIMEWARPUPDATERQUEUE);
            }
            return services;
        }

    }
}
