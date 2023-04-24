using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.DependencyInjection;
using ETHTPS.API.Security.Core.Authentication;
using ETHTPS.API.Security.Core.Policies;
using ETHTPS.Data.Integrations.InfluxIntegration;

using NLog.Extensions.Hosting;

using static ETHTPS.TaskRunner.Constants;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNLog();
var services = builder.Services;
services.AddEssentialServices()
        .AddDatabaseContext(CURRENT_APP_NAME)
        .AddDataProviderServices(DatabaseProvider.MSSQL)
        .AddMixedCoreServices()
        .AddCustomCORSPolicies()
        .AddAPIKeyAuthenticationAndAuthorization()
        .AddControllers()
        .AddControllersAsServices();

services.AddSwagger()
        .AddScoped<IInfluxWrapper, InfluxWrapper>()
        .AddDataUpdaterStatusService()
        .AddDataServices()
        .WithStore(DatabaseProvider.MSSQL)
        .AddRunner(BackgroundServiceType.Coravel);
;//.RegisterMicroservice(CURRENT_APP_NAME, "Task runner web app");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.ConfigureSwagger();
app.UseAuthorization();
app.MapControllers();
app.UseRunner(BackgroundServiceType.Coravel);
app.Run();
