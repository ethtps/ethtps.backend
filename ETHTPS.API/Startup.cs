

using Coravel;

using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.Core.Middlewares;
using ETHTPS.API.DependencyInjection;
using ETHTPS.API.Security.Core.Authentication;
using ETHTPS.API.Security.Core.Policies;
using ETHTPS.Configuration.Database;
using ETHTPS.Services.BackgroundTasks.Recurring.Aggregated;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ETHTPS.API
{
    public sealed class Startup
    {
        private readonly string _myAllowSpecificOrigins = "_myAllowSpecificOrigins";
        private readonly string _appName = "ETHTPS.API.General";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEssentialServices();
            services.AddDatabaseContext(_appName);
            services.AddCustomCORSPolicies();
            services.AddResponseCompression();
            services.AddControllersWithViews()
                    .AddControllersAsServices()
                    .ConfigureNewtonsoftJson();
            services.AddSwagger()
                    .AddMemoryCache()
                    .AddAPIKeyProvider()
                    .AddAPIKeyAuthenticationAndAuthorization()
                    .AddDataProviderServices(DatabaseProvider.MSSQL)
                    .AddMixedCoreServices()
                    .AddQueue()
                    .AddCache()
                    .AddScoped<AggregatedEndpointStatsBuilder>()
                    .AddInfluxHistoricalDataProvider() //Not working r/n
                    .AddMSSQLHistoricalDataServices()
                    .AddRedisCache();
            //.RegisterMicroservice(APP_NAME, "General API");
            services.AddDataUpdaterStatusService();

#if DEBUG
            services.AddScoped<PublicDataInitializer>();
#endif
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.RequestsAreForwardedByReverseProxy();
            //app.UseMiddleware<UnstableConnectionSimulatorMiddleware>(); //Simulating high server load
            app.UseMiddleware<AccesStatsMiddleware>();
            app.ConfigureSwagger();
            app.UseRouting();
            app.UseMiddleware<RedisCacheMiddleware>();
            app.UseAuthorization();
            app.UseCors(_myAllowSpecificOrigins);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
        }
    }
}
