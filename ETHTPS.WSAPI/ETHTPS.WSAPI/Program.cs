using Coravel;

using ETHTPS.API.DependencyInjection;
using ETHTPS.API.Security.Core.Policies;
using ETHTPS.WSAPI.Infrastructure.LiveData.Connection;

namespace ETHTPS.WSAPI
{
    public sealed class Program
    {
        const string _APP_NAME = "ETHTPS.WSAPI";
        private const string _myAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            var services = builder.Services;
            services.AddEssentialServices();
            services.AddDatabaseContext(_APP_NAME);
            services.AddCustomCORSPolicies();
            services.AddControllersWithViews()
                    .AddControllersAsServices()
                    .ConfigureNewtonsoftJson();
            services.AddSwagger("ETHTPS.WSAPI", "Backend definition for ETHTPS's SignalR API; you're free to play around :)", false)
                   .AddMemoryCache()
                   .AddQueue()
                   .AddCache()
                   .AddScheduler()
                   .AddMSSQLHistoricalDataServices();
            services.AddSignalR();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
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