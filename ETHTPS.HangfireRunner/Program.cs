using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.DependencyInjection;
using ETHTPS.API.Security.Core.Policies;
using ETHTPS.Configuration;
using ETHTPS.Data.Integrations.InfluxIntegration;
using ETHTPS.Services.BackgroundTasks.Static.WSAPI;
using ETHTPS.Services.Infrastructure.Messaging;
using ETHTPS.Services.LiveData;
using ETHTPS.TaskRunner.BackgroundServices;

using Hangfire;

using NLog.Extensions.Hosting;

using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

using static ETHTPS.TaskRunner.Constants;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddServiceDiscovery(options =>
{
    options.UseConsul();
});
builder.Host.UseNLog();
var services = builder.Services;
services.AddEssentialServices()
        .AddDatabaseContext(CURRENT_APP_NAME)
        .AddMixedCoreServices()
        .AddCustomCORSPolicies()
        .AddControllers()
        .AddControllersAsServices();

var runnerType = BackgroundServiceType.Hangfire;

services.AddSwagger()
        .AddScoped<IInfluxWrapper, InfluxWrapper>()
        .AddDataUpdaterStatusService()
        .AddDataServices()
        .AddRunner(runnerType, CURRENT_APP_NAME)
        .WithStore(DatabaseProvider.MSSQL, CURRENT_APP_NAME)
        .AddDataProviderServices(DatabaseProvider.MSSQL)
        .AddRabbitMQMessagePublisher()
        .AddScoped<WSAPIPublisher>()
        .AddScoped<NewDatapointPublisherTask>();
services.AddHostedService<NewDatapointHandler>(x =>
{
    using (var scope = x.CreateScope())
    {
        return new NewDatapointHandler(
        scope.ServiceProvider.GetRequiredService<ILogger<NewDatapointHandler>>(),
        scope.ServiceProvider.GetRequiredService<IDBConfigurationProvider>(),
        scope.ServiceProvider.GetRequiredService<IMessagePublisher>());
    }
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    //scope.ServiceProvider.GetRequiredService<EthtpsContext>().DeleteAllJobsAsync().Wait();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseHangfireDashboard();
app.ConfigureSwagger();
app.UseRunner(runnerType);
app.MapControllers();
app.Run();
