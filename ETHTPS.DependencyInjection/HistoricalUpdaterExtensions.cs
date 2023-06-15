using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Integrations.InfluxIntegration;
using ETHTPS.Data.Integrations.InfluxIntegration.HistoricalDataProviders;
using ETHTPS.Services;
using ETHTPS.Services.BlockchainServices.HangfireLogging;
using ETHTPS.Services.Ethereum;
using ETHTPS.Services.Ethereum.JSONRPC.Infura;
using ETHTPS.Services.Ethereum.Starkware;
using ETHTPS.Services.Infrastructure.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using static ETHTPS.API.Core.Constants;

namespace ETHTPS.API.DependencyInjection
{
    public static class HistoricalUpdaterExtensions
    {
        public static IServiceCollection AddHistoricalBlockInfoDataUpdaters(this IServiceCollection services, string[] configurationQueues)
        {
            if (configurationQueues.Contains(HISTORICALUPDATERQUEUE))
            {
                //services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<EtherscanBlockInfoProvider>, EtherscanBlockInfoProvider>(CronConstants.EveryMidnight, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<InfuraBlockInfoProviderBase>, InfuraBlockInfoProviderBase>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<MetisBlockInfoProvider>, MetisBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<ArbitrumBlockInfoProvider>, ArbitrumBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<OptimismBlockInfoProvider>, OptimismBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<Services.Ethereum.JSONRPC.Infura.PolygonBlockInfoProvider>, Services.Ethereum.JSONRPC.Infura.PolygonBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<XDAIHTTPBlockInfoProvider>, XDAIHTTPBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<ZKSwapBlockInfoProvider>, ZKSwapBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<ZKSsyncBlockInfoProvider>, ZKSsyncBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<AVAXBlockInfoProvider>, AVAXBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<BobaNetworkBlockInfoProvider>, BobaNetworkBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<LoopringBlockInfoProvider>, LoopringBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<AztecBlockInfoProvider>, AztecBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<VoyagerBlockInfoProvider>, VoyagerBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<Nahmii20BlockInfoProvider>, Nahmii20BlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireDateBackgroundService<HangfireDateHistoricalBlockInfoProviderDataLogger<SorareBlockInfoProvider>, SorareBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireDateBackgroundService<HangfireDateHistoricalBlockInfoProviderDataLogger<DeversiFIHTTPBlockInfoProvider>, DeversiFIHTTPBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<PolygonHermezBlockInfoProvider>, PolygonHermezBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<ZKSsyncEraBlockInfoProvider>, ZKSsyncEraBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
                //services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<HabitatBlockInfoProvider>, HabitatBlockInfoProvider>(CronConstants.Never, HISTORICALUPDATERQUEUE);
                //services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<BSCScanBlockInfoProvider>, BSCScanBlockInfoProvider>(CronConstants.Never, HISTORICALUPDATERQUEUE);
            }
            return services;
        }

        public static IServiceCollection AddInfluxHistoricalDataProvider(this IServiceCollection services)
        {
            services.TryAddScoped<IInfluxWrapper, InfluxWrapper>();
            return services.AddScoped<IAsyncHistoricalBlockInfoProvider, HistoricalInfluxProvider>();
        }
    }
}
