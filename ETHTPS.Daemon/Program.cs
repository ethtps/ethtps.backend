using System.Text;

using ETHTPS.API.DependencyInjection;
using ETHTPS.Configuration;
using ETHTPS.Configuration.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using NLog.Extensions.Hosting;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using Utility.CommandLine;

namespace ETHTPS.Daemon
{
    public class Program
    {
        [Argument('s', "scope", "Scope of the daemon")]
        private static string? Scope { get; set; }

        [Argument('v', "verbose", "Verbose output")]
        private static bool Verbose { get; set; } = false;

        private static string[] _allowedScopes = new[]
        {
            "BlockDataAggregator"
        };

        static void Main(string[] args)
        {
            Arguments.Populate();
            if (string.IsNullOrWhiteSpace(Scope))
            {
                Log("Please specify a scope using -s or --scope");
                return;
            }
            if (!_allowedScopes.Contains(Scope))
            {
                Log($"Scope must be one of the following: [{string.Join(" | ", _allowedScopes)}]");
                return;
            }

            var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { });
            builder.Host.UseNLog();
            var services = builder.Services;
            services.AddEssentialServices()
                    .AddDatabaseContext("ETHTPS.Tests")
                    .AddMixedCoreServices();

            var serviceProvider = services.BuildServiceProvider();

            Log($"Starting ETHTPS daemon in {Scope} mode...");
            IDBConfigurationProvider? configProvider = serviceProvider.GetRequiredService<IDBConfigurationProvider>();

            LogVerbose("Connecting to RabbitMQ...");
            var factory = new ConnectionFactory { HostName = configProvider.GetFirstConfigurationString("RabbitMQ_Host_Dev") };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            LogVerbose("Connected to RabbitMQ");
            channel.QueueDeclare(queue: Scope,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            LogVerbose("Declared queue for scope");
            var consumer = new EventingBasicConsumer(channel);
            LogVerbose("Created consumer");
            var cancel = false;
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Log("Caught exit signal");
                cancel = true;
                eventArgs.Cancel = true;
            };
            Log("Listening for messages...");
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Log($" [x] Received {message}");
            };
            while (!cancel) Thread.Sleep(100);
            Log("Exiting...");
            channel.Close();
            connection.Close();
        }

        private static void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {message}");
        }

        private static void LogVerbose(string message)
        {
            if (Verbose)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {message}");
                Console.ResetColor();
            }
        }
    }
}