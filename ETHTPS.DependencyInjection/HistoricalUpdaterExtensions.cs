using ETHTPS.Services.Ethereum.Scan.Implementations;
using ETHTPS.Services.Ethereum.Starkware;
using ETHTPS.Services.Ethereum;
using ETHTPS.Services;
using Microsoft.Extensions.DependencyInjection;
using static ETHTPS.API.Core.Constants;
using ETHTPS.Services.Infrastructure.Extensions;
using ETHTPS.Data.Integrations.MSSQL.HistoricalDataServices;
using ETHTPS.Services.Ethereum.JSONRPC.Infura;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Integrations.InfluxIntegration.HistoricalDataServices;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ETHTPS.Data.Integrations.InfluxIntegration;
using ETHTPS.Services.BlockchainServices.HangfireLogging;

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
                services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<PolygonBlockInfoProvider>, PolygonBlockInfoProvider>(CronConstants.NEVER, HISTORICALUPDATERQUEUE);
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
                //services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<HabitatBlockInfoProvider>, HabitatBlockInfoProvider>(CronConstants.Never, HISTORICALUPDATERQUEUE);
                //services.RegisterHistoricalHangfireBackgroundService<HangfireHistoricalBlockInfoProviderDataLogger<BSCScanBlockInfoProvider>, BSCScanBlockInfoProvider>(CronConstants.Never, HISTORICALUPDATERQUEUE);
            }
            return services;
        }


        public static IServiceCollection AddMSSQLHistoricalDataServices(this IServiceCollection services)
        {
            services.AddScoped<IHistoricalDataProvider, OneHourHistoricalDataProvider>();
            services.AddScoped<IHistoricalDataProvider, OneDayHistoricalDataProvider>();
            services.AddScoped<IHistoricalDataProvider, OneWeekHistoricalDataProvider>();
            services.AddScoped<IHistoricalDataProvider, OneMonthHistoricalDataProvider>();
            services.AddScoped<IHistoricalDataProvider, OneYearHistoricalDataProvider>();
            services.AddScoped<IHistoricalDataProvider, AllHistoricalDataProvider>();
            services.AddScoped<IHistoricalDataProvider, InstantDataProvider>();
            services.AddScoped<IHistoricalDataProvider, OneMinuteHistoricalDataProvider>();
            return services;
        }

        public static IServiceCollection AddInfluxHistoricalDataProvider(this IServiceCollection services)
        {
            services.TryAddScoped<IInfluxWrapper, InfluxWrapper>();
            return services.AddScoped<IAsyncHistoricalBlockInfoProvider, HistoricalInfluxProvider>();
        }
    }
}
