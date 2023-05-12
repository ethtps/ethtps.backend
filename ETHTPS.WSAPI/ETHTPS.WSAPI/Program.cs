using Coravel;

using ETHTPS.API.Core.Services.LiveData;
using ETHTPS.API.DependencyInjection;
using ETHTPS.API.Security.Core.Policies;
using ETHTPS.WSAPI.Infrastructure.LiveData.Connection;

using NLog.Extensions.Hosting;

using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

namespace ETHTPS.WSAPI
{
    public sealed class Program
    {
        const string _APP_NAME = "ETHTPS.WSAPI";
        private const string _myAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseNLog();
            builder.Host.AddServiceDiscovery(options =>
            {
                options.UseConsul();
            });
            // Add services to the container.
            builder.Services.AddRazorPages();
            var services = builder.Services;
            services.AddEssentialServices();
            services.AddDatabaseContext(_APP_NAME);
            services.AddCustomCORSPolicies();
            services.AddControllersWithViews()
                    .AddControllersAsServices()
                    .AddNewtonsoftJson()
                    .ConfigureNewtonsoftJson();
            services.AddSwagger("ETHTPS.WSAPI", "Backend definition for ETHTPS's SignalR API; you're free to play around :)", false)
                   .AddMemoryCache()
                   .AddQueue()
                   .AddCache()
                   .AddScheduler()
                   .AddEvents()
                   .AddMSSQLHistoricalDataServices()
                   .AddSingleton<LiveDataAggregator>();
            services.AddSignalR();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            var eventRegistration = app.Services.ConfigureEvents();
            eventRegistration
                .Register<LiveDataChanged>()
                .Subscribe<LiveDataHub>();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.RequestsAreForwardedByReverseProxy();
            app.MapRazorPages();
            app.ConfigureSwagger();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(_myAllowSpecificOrigins);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
            app.MapHub<LiveDataHub>("/api/v3/wsapi/live-data");
            app.Services.UseScheduler(scheduler =>
            {

            });
            app.Run();
        }
    }
}