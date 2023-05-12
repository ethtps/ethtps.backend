using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.DependencyInjection;
using ETHTPS.Configuration;
using ETHTPS.Configuration.ProviderConfiguration;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Integrations.InfluxIntegration;
using ETHTPS.Data.Integrations.InfluxIntegration.HistoricalDataServices;
using ETHTPS.Services.BlockchainServices.BlockTime;
using ETHTPS.Services.LiveData;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using NLog.Extensions.Hosting;

using Steeltoe.Common.Http.Discovery;

namespace ETHTPS.Tests
{
    /// <summary>
    /// A base test class for the BIL part of the framework
    /// </summary>
    public abstract class TestBase
    {
        protected ServiceProvider ServiceProvider { get; private set; }
        protected IDBConfigurationProvider ConfigurationProvider => ServiceProvider.GetRequiredService<IDBConfigurationProvider>();

        const string APP_NAME = "ETHTPS.Tests";
        protected TestBase()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { });
            builder.Host.UseNLog();
            var services = builder.Services;
            services
                    .AddEssentialServices()
                    .AddDatabaseContext(APP_NAME)
                    .AddMixedCoreServices()
                    .AddDataProviderServices(DatabaseProvider.MSSQL)
                    .AddDataUpdaterStatusService()
                    .AddScoped<IInfluxWrapper, InfluxWrapper>()
                    .AddScoped<IAsyncHistoricalBlockInfoProvider, HistoricalInfluxProvider>()
                    .AddMSSQLHistoricalDataServices()
                    .AddTransient<IProviderConfigurationService, ProviderConfigurationService>()
                    .AddRedisCache()
                    .AddSingleton<EthereumBlockTimeProvider>()
                    .AddSingleton<WSAPIPublisher>()
                    .AddHttpClient("ETHTPS-WSAPI")
                    .AddServiceDiscovery()
                    .AddTypedClient<WSAPIPublisher>();
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
