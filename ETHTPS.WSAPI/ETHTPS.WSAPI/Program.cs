using Coravel;

using ETHTPS.API.Core.Services.LiveData;
using ETHTPS.API.DependencyInjection;
using ETHTPS.API.Security.Core.Policies;
using ETHTPS.Data.Core;
using ETHTPS.Services.Infrastructure.Messaging;
using ETHTPS.WSAPI.BackgroundServices;
using ETHTPS.WSAPI.Infrastructure.LiveData.Connection;

using NLog.Extensions.Hosting;

namespace ETHTPS.WSAPI
{
    public sealed class Program
    {
        private const string _myAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseNLog();
            // Add services to the container.
            builder.Services.AddRazorPages();
            var services = builder.Services;
            services.AddEssentialServices();
            services.AddDatabaseContext(ETHTPSMicroservice.WSAPI);
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
                   .AddRabbitMQMessagePublisher();
            services.AddSignalR();

            services.AddHostedService<LiveDataService>();
            var app = builder.Build();
            app.UseCustomCORSPolicies();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            var eventRegistration = app.Services.ConfigureEvents();
            eventRegistration
                .Register<LiveDataChanged>();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
                endpoints.MapHub<LiveDataHub>("/api/v3/wsapi/live-data");
            });
            //app.UseAuthorization();
            app.RequestsAreForwardedByReverseProxy();
            app.MapRazorPages();
            app.ConfigureSwagger();
            app.Run();
        }
    }
}