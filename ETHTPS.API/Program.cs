using System.IO;
using System.Reflection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using NLog.Extensions.Hosting;

using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

namespace ETHTPS.API
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(configureDelegate => configureDelegate.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
                //.AddDiscoveryClient()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .UseNLog()
            .AddServiceDiscovery(o => o.UseConsul());
    }
}
