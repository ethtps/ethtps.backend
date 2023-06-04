using System.Diagnostics;
using System.Text;

using ETHTPS.Configuration;
using ETHTPS.Configuration.Extensions;
using ETHTPS.Data.Core.Models.LiveData.Triggers;
using ETHTPS.Services;
using ETHTPS.Services.Infrastructure.Messaging;
using ETHTPS.Services.LiveData;
using ETHTPS.WSAPI.Infrastructure.LiveData.Connection;

using Microsoft.AspNetCore.SignalR;

using Newtonsoft.Json;

using RabbitMQ.Client.Events;

namespace ETHTPS.WSAPI.BackgroundServices
{
    /// <summary>
    /// Listens to the live data queue and sends new entries to SignalR clients.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Hosting.IHostedService" />
    public sealed class LiveDataService : IHostedService
    {
        private readonly RabbitMQSubscriptionService _subscriptionService;
        private readonly ILogger<LiveDataService> _logger;
        private readonly IDBConfigurationProvider _configurationProvider;
        private readonly IServiceScope _scope;
        private bool _cancel = false;
        private readonly IHubContext<LiveDataHub> _hubContext;
        private readonly LatestEntryAggregator<string, L2DataUpdateModel> _latestEntryAggregator = new();
        private static Func<L2DataUpdateModel, L2DataUpdateModel, bool> _DIFF_SELECTOR = (x, y) => x.Data?.TPS != y.Data?.TPS || x.Data?.GPS != y.Data?.GPS;
        private static Func<L2DataUpdateModel, string> _KEY_SELECTOR = x => x.Provider;

        public LiveDataService(IServiceProvider services, ILogger<LiveDataService> logger)
        {
            _scope = services.CreateScope();
            _logger = logger;
            _configurationProvider = _scope.ServiceProvider.GetRequiredService<IDBConfigurationProvider>();
            _hubContext = _scope.ServiceProvider.GetRequiredService<IHubContext<LiveDataHub>>();

            _subscriptionService = new RabbitMQSubscriptionService(new RabbitMQSubscriptionConfig()
            {
                AutoAck = false,
                AutoDelete = false,
                QueueName = MessagingQueues.LIVEDATA_MULTIPLENEWDATAPOINTS_QUEUE,
                Host = _configurationProvider.GetFirstConfigurationString("RabbitMQ_Host_Dev")
            });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            DoWork(cancellationToken);
            await Task.CompletedTask;
        }

        private async void DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("LiveDataService: listening...");
            PublishLatestEntriesAsync(cancellationToken, 2500);
            async void handler(object? sender, BasicDeliverEventArgs e)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                try
                {
                    var receivedMessage = Encoding.UTF8.GetString(e.Body.ToArray());

                    var datapoints = JsonConvert.DeserializeObject<L2DataUpdateModel[]>(receivedMessage);
                    if (datapoints?.Length > 0)
                    {
                        var newAggregatedData = new LatestEntryAggregator<string, L2DataUpdateModel>(datapoints, _KEY_SELECTOR);
                        var diff = _latestEntryAggregator.Diff(newAggregatedData, _DIFF_SELECTOR);
                        //_logger.LogInformation($"LiveDataService: {diff?.Count()} new entries");
                        if (diff?.Count() > 0)
                        {
                            _latestEntryAggregator.Clear();
                            _latestEntryAggregator.Push(diff, _KEY_SELECTOR);

                            await _hubContext.Clients.All.SendCoreAsync("LiveDataChanged", new[] { _latestEntryAggregator.ToDictionary(x => x.Provider, x => x) }, cancellationToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from RabbitMQ and sending live updates to clients");
                }
                finally
                {
                    _subscriptionService.Channel.BasicAck(e.DeliveryTag, true);
                    stopwatch.Stop();
                    _logger.LogInformation($"LiveDataService: processed message in {stopwatch.ElapsedMilliseconds}ms");
                }
            };
            _subscriptionService.MessageReceived += handler;
            while (!_cancel && !cancellationToken.IsCancellationRequested) await Task.Delay(100, cancellationToken);
            _subscriptionService.MessageReceived -= handler;
            _logger.LogInformation("LiveDataService: stopped");
        }

        private async void PublishLatestEntriesAsync(CancellationToken cancellationToken, int msInterval)
        {
            while (!_cancel && !cancellationToken.IsCancellationRequested)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error publishing latest entries");
                }
                await Task.Delay(msInterval, cancellationToken);
            }
            _logger.LogInformation("Stopped aggregator publisher task");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancel = true;
            _scope.Dispose();
            return Task.CompletedTask;
        }
    }
}
