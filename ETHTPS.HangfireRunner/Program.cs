using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.DependencyInjection;
using ETHTPS.API.Security.Core.Policies;
using ETHTPS.Configuration;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Integrations.InfluxIntegration;
using ETHTPS.Services.Infrastructure.Messaging;
using ETHTPS.Services.LiveData;
using ETHTPS.TaskRunner.BackgroundServices;

using NLog.Extensions.Hosting;

using static ETHTPS.Services.Infrastructure.Messaging.MessagingExtensions;
using static ETHTPS.TaskRunner.Constants;
using static ETHTPS.Utils.Logging.LoggingUtils;

Trace($"Starting {Microservice.TaskRunner.GetFullName()}...");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNLog();
var services = builder.Services;
services.AddEssentialServices()
        .AddDatabaseContext(CURRENT_APP)
        .AddMixedCoreServices()
        .AddCustomCORSPolicies()
        .AddControllers()
        .AddControllersAsServices();
Trace("Added essential services");

var runnerType = BackgroundServiceType.Hangfire;

services.AddSwagger()
        .AddScoped<IInfluxWrapper, InfluxWrapper>()
        .AddDataUpdaterStatusService()
        .AddDataServices()
        .AddRunner(runnerType, CURRENT_APP, DatabaseProvider.InMemory)
        .WithStore(DatabaseProvider.InfluxDB, CURRENT_APP)
        .AddDataProviderServices(DatabaseProvider.InfluxDB)
        .AddRabbitMQMessagePublisher()
        .AddRabbitMQSubscriptionService(AllowedSubscriptionScope.BlockDataAggregator)
        .AddScoped<WSAPIPublisher>();

Trace("Added dependencies");
services.AddHostedService(x =>
{
    using var scope = x.CreateScope();
    return new NewDatapointHandler(
        scope.ServiceProvider.GetRequiredService<ILogger<NewDatapointHandler>>(),
        scope.ServiceProvider.GetRequiredService<DBConfigurationProviderWithCache>(),
        scope.ServiceProvider.GetRequiredService<IMessagePublisher>());
});
Trace("Registered hosted services");
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.ConfigureSwagger();
app.MapControllers();
app.UseTaskRunner(runnerType);
Trace("All set");
app.Run();
