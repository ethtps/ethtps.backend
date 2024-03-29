using NLog.Extensions.Logging;

using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ETHTPS.API.Gateway
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder()
            .UseUrls(args)
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                    .AddJsonFile("ocelot.json")
                    .AddEnvironmentVariables();
            })
            .ConfigureServices(s =>
            {
                s.AddOcelot();
            })
            .UseIISIntegration()
            .ConfigureLogging(opts =>
            {
                opts.AddNLog();
            })
            .Configure(app =>
            {
                app.UseOcelot().Wait();
            })
            .Build()
            .Run();
        }
    }
}

