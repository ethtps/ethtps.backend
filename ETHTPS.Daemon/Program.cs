using System.Text;

using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.API.DependencyInjection;
using ETHTPS.Configuration;
using ETHTPS.Configuration.Extensions;
using ETHTPS.Daemon.Infra;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Services.Infrastructure.Messaging;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using NLog.Extensions.Hosting;

using Utility.CommandLine;

namespace ETHTPS.Daemon
{
    public sealed class Program
    {
        [Argument('s', "scope", "Scope of the daemon")]
        private static string? Scope { get; set; }

        [Argument('v', "verbose", "Verbose output")]
        private static bool Verbose { get; set; } = false;

        [Argument('l', "limit", "Task limit")]
        public static int TaskLimit { get; set; } = 10;

        public static int RunningTasks = 0;

        public static bool SlotAvailable => RunningTasks < TaskLimit;

        private static string[] _allowedScopes = new[]
        {
            "BlockDataAggregator"
        };

        private static Dictionary<string, string> _queueCorrespondence = new()
        {
            { _allowedScopes[0], "L2DataRequestQueue" }
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
                    .AddMixedCoreServices()
                    .AddRabbitMQMessagePublisher()
                    .AddRedisCache()
                    .AddSingleton<IRabbitMQSubscriptionService>(x =>
                    {
                        using (var scope = x.CreateScope())
                            return new RabbitMQSubscriptionService(new RabbitMQSubscriptionConfig()
                            {
                                AutoAck = false,
                                QueueName = _queueCorrespondence[Scope],
                                Host = x.GetRequiredService<IDBConfigurationProvider>().GetFirstConfigurationString("RabbitMQ_Host_Dev")
                            });
                    });

            var serviceProvider = services.BuildServiceProvider();
            var consumer = serviceProvider.GetRequiredService<IRabbitMQSubscriptionService>();
            var channel = consumer.Channel;
            consumer.MessageReceived += (model, ea) =>
            {
                try
                {
                    if (!SlotAvailable)
                    {
                        LogVerbose($"[{ea.RoutingKey}]: No slots available, skipping message");
                        channel.BasicNack(ea.DeliveryTag, false, true);
                        return;
                    }
                    Interlocked.Increment(ref RunningTasks);
                    channel.BasicAck(ea.DeliveryTag, false);
                    LogVerbose($"[{ea.RoutingKey}][{ea.DeliveryTag}] ack");

                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Log($"[{ea.RoutingKey}]: Received message");
                    switch (ea.RoutingKey)
                    {
                        case "L2DataRequestQueue":
                            LogVerbose($"[{ea.RoutingKey}]: Processing message");
                            var request = JsonConvert.DeserializeObject<L2DataRequestModel>(message);
                            var requestKey = request?.ToCacheKey();
                            if (string.IsNullOrWhiteSpace(requestKey))
                            {
                                LogVerbose($"[{ea.RoutingKey}]: Invalid request - no request key");
                                return;
                            }
                            var preprocessor = new DataRequestPreprocessor(requestKey, serviceProvider.GetRequiredService<IMessagePublisher>(), serviceProvider.GetRequiredService<IRedisCacheService>(), request);
                            Task.Run(async () =>
                            {
                                try
                                {
                                    await preprocessor.RunAsync();
                                }
                                catch (Exception e)
                                {
                                    Log($"Exception occurred while processing queue message: {e}");
                                }
                                finally
                                {
                                    Interlocked.Decrement(ref RunningTasks);
                                    LogVerbose($"Released slot");
                                    LogVerbose($"Slots available: {TaskLimit - RunningTasks}");
                                }
                            });
                            break;
                    }
                }
                catch (Exception e)
                {
                    Log($"Exception occurred while processing queue message: {e}");
                }
            };
            var cancel = false;
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Log("Caught exit signal");
                cancel = true;
                eventArgs.Cancel = true;
            };
            Log("Listening...");
            while (!cancel) Thread.Sleep(100);
            Log("Exiting...");
            channel.Close();
        }

        public static void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {message}");
        }

        public static void LogVerbose(string message)
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