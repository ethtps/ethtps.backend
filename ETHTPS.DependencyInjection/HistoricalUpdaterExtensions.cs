using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Integrations.InfluxIntegration;
using ETHTPS.Data.Integrations.InfluxIntegration.HistoricalDataProviders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ETHTPS.API.DependencyInjection
{
    public static class HistoricalUpdaterExtensions
    {
        public static IServiceCollection AddInfluxHistoricalDataProvider(this IServiceCollection services)
        {
            services.TryAddScoped<IInfluxWrapper, InfluxWrapper>();
            return services.AddScoped<IAsyncHistoricalBlockInfoProvider, HistoricalInfluxProvider>();
        }
    }
}
