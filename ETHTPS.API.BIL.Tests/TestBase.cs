using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.DependencyInjection;
using ETHTPS.Configuration.ProviderConfiguration;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Integrations.InfluxIntegration;
using ETHTPS.Data.Integrations.InfluxIntegration.HistoricalDataServices;

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
        protected TestBase()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { });
            builder.Host.UseNLog();
            var services = builder.Services;
            services
                    .AddEssentialServices()
                    .AddDatabaseContext("ETHTPS.Tests")
                    .AddMixedCoreServices()
                    .AddDataProviderServices(DatabaseProvider.MSSQL)
                    .AddDataUpdaterStatusService()
                    .AddScoped<IInfluxWrapper, InfluxWrapper>()
                    .AddScoped<IAsyncHistoricalBlockInfoProvider, HistoricalInfluxProvider>()
                    .AddMSSQLHistoricalDataServices()
                    .AddTransient<IProviderConfigurationService, ProviderConfigurationService>()
                    .AddRedisCache();
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
