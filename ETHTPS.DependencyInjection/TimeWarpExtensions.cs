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

            }
            return services;
        }

    }
}
