using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.DependencyInjection;
using ETHTPS.API.Security.Core.Policies;
using ETHTPS.Data.Integrations.InfluxIntegration;
using ETHTPS.Data.Integrations.MSSQL;

using NLog.Extensions.Hosting;

using static ETHTPS.TaskRunner.Constants;

var builder = WebApplication.CreateBuilder(args);
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
        .AddDataProviderServices(DatabaseProvider.MSSQL);
;//.RegisterMicroservice(CURRENT_APP_NAME, "Task runner web app");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<EthtpsContext>().DeleteAllJobsAsync().Wait();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.ConfigureSwagger();
app.UseRunner(runnerType);
app.MapControllers();
app.Run();
