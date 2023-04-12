using Coravel;

using ETHTPS.API.DependencyInjection;
using ETHTPS.API.Security.Core.Policies;

namespace ETHTPS.WSAPI
{
    public class Program
    {
        const string APP_NAME = "ETHTPS.WSAPI";
        private const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.AddRazorPages();
            var services = builder.Services;
            services.AddEssentialServices();
            services.AddDatabaseContext(APP_NAME);
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
            app.UseCors(MyAllowSpecificOrigins);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
            app.Services.UseScheduler(scheduler =>
            {

            });
            app.Run();
        }
    }
}