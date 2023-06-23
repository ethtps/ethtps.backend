using ETHTPS.API.BIL.Infrastructure.Services;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.DependencyInjection;
using ETHTPS.Configuration;
using ETHTPS.Configuration.ProviderConfiguration;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Integrations.InfluxIntegration;
using ETHTPS.Data.Integrations.InfluxIntegration.HistoricalDataProviders;
using ETHTPS.Data.Integrations.InfluxIntegration.ProviderServices.DataProviders;
using ETHTPS.Services.BlockchainServices.BlockTime;
using ETHTPS.Services.Infrastructure.Messaging;
using ETHTPS.Services.LiveData;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using NLog.Extensions.Hosting;

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
        const DatabaseProvider _DATABASE_PROVIDER = DatabaseProvider.InfluxDB;
        protected TestBase()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { });
            builder.Host.UseNLog();
            var services = builder.Services;
            services
                    .AddEssentialServices()
                    .AddDatabaseContext(APP_NAME)
                    .AddMixedCoreServices()
                    .AddScoped<IPSDataFormatter, DeedleTimeSeriesFormatter>()
                    .AddDataProviderServices(_DATABASE_PROVIDER)
                    .WithStore(_DATABASE_PROVIDER, APP_NAME)
                    .AddDataUpdaterStatusService()
                    .AddScoped<IInfluxWrapper, InfluxWrapper>()
                    .AddScoped<IAsyncHistoricalBlockInfoProvider, HistoricalInfluxProvider>()
                    .AddTransient<IProviderConfigurationService, ProviderConfigurationService>()
                    .AddRedisCache()
                    .AddRabbitMQMessagePublisher()
                    .AddSingleton<EthereumBlockTimeProvider>()
                    .AddScoped<WSAPIPublisher>()
                    .AddScoped<InfluxTPSService>()
                    .AddScoped<InfluxGPSService>()
                    .AddScoped<InfluxGTPSService>();
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
