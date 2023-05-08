using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.DependencyInjection;
using ETHTPS.API.Security.Core.Policies;
using ETHTPS.Data.Integrations.InfluxIntegration;

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
        .WithStore(DatabaseProvider.MSSQL, CURRENT_APP_NAME)
        .AddRunner(runnerType)
        .AddDataProviderServices(DatabaseProvider.MSSQL);
;//.RegisterMicroservice(CURRENT_APP_NAME, "Task runner web app");

var app = builder.Build();

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
