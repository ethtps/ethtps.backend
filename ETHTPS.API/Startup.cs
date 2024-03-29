

using Coravel;

using EntityGraphQL.AspNet;

using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.Core.Middlewares;
using ETHTPS.API.DependencyInjection;
using ETHTPS.API.Security.Core.Authentication;
using ETHTPS.API.Security.Core.Policies;
using ETHTPS.Data.Core;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Services.BackgroundTasks.Recurring.Aggregated;
using ETHTPS.Services.Infrastructure.Messaging;

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
        private readonly Microservice _app = Microservice.API;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEssentialServices();
            services.AddDatabaseContext(_app);
            services.AddCustomCORSPolicies();
            services.AddResponseCompression();
            services.AddControllersWithViews()
                    .AddControllersAsServices()
                    .ConfigureNewtonsoftJson();
            services.AddSwagger()
                    .AddMemoryCache()
                    .AddAPIKeyProvider()
                    .AddAPIKeyAuthenticationAndAuthorization()
                    .AddDataProviderServices(DatabaseProvider.InfluxDB)
                    .WithStore(DatabaseProvider.InfluxDB, Microservice.API)
                    .AddMixedCoreServices()
                    .AddQueue()
                    .AddCache()
                    .AddScoped<AggregatedEndpointStatsBuilder>()
                    .AddInfluxHistoricalDataProvider()
                    .AddRabbitMQMessagePublisher()
                    .AddGraphQLSchema<EthtpsContext>();
            services.AddDataUpdaterStatusService();
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
                /*
                endpoints.MapGraphQL<EthtpsContext>(options: new ExecutionOptions()
                {
                    EnableQueryCache = true,
                    ExecuteServiceFieldsSeparately = true,
                });*/
            });
        }
    }
}
