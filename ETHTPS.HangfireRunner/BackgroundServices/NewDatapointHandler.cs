using System.Diagnostics;
using System.Text;

using ETHTPS.Configuration;
using ETHTPS.Configuration.Extensions;
using ETHTPS.Data.Core.Models.LiveData.Triggers;
using ETHTPS.Services;
using ETHTPS.Services.Infrastructure.Messaging;
using ETHTPS.Services.LiveData;

using Newtonsoft.Json;

using RabbitMQ.Client.Events;

namespace ETHTPS.TaskRunner.BackgroundServices
{
    /// <summary>
    /// Handles new datapoints received from RabbitMQ, aggregates them by keeping only the latest entry and publishes them to the clients through WSAPI
    /// </summary>
    public sealed class NewDatapointHandler : IHostedService
    {
        private readonly RabbitMQSubscriptionService _subscriptionService;
        private readonly ILogger<NewDatapointHandler> _logger;
        private readonly IDBConfigurationProvider _configurationProvider;
        private readonly LatestEntryAggregator<string, L2DataUpdateModel> _latestEntryAggregator = new LatestEntryAggregator<string, L2DataUpdateModel>();
        private readonly IMessagePublisher _messagePublisher;
        private bool _cancel = false;

        public NewDatapointHandler(ILogger<NewDatapointHandler> logger, IDBConfigurationProvider configurationProvider, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _configurationProvider = configurationProvider;
            _messagePublisher = messagePublisher;

            _subscriptionService = new RabbitMQSubscriptionService(new RabbitMQSubscriptionConfig()
            {
                AutoAck = false,
                AutoDelete = false,
                QueueName = MessagingQueues.LIVEDATA_NEWDATAPOINT_QUEUE,
                Host = configurationProvider.GetFirstConfigurationString("RabbitMQ_Host_Dev")
            });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("NewDatapointHandler: listening...");
            PublishLatestEntriesAsync(cancellationToken, 2500);
            void handler(object? sender, BasicDeliverEventArgs e)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                try
                {
                    var receivedMessage = Encoding.UTF8.GetString(e.Body.ToArray());

                    var datapoint = JsonConvert.DeserializeObject<L2DataUpdateModel>(receivedMessage);
                    if (datapoint != null)
                    {
                        _latestEntryAggregator.Push(datapoint.Provider, datapoint);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from RabbitMQ");
                }
                finally
                {
                    _subscriptionService.Channel.BasicAck(e.DeliveryTag, true);
                    stopwatch.Stop();
                    _logger.LogInformation($"NewDatapointHandler: processed message in {stopwatch.ElapsedMilliseconds}ms");
                }
            };
            _subscriptionService.MessageReceived += handler;
            while (!_cancel && !cancellationToken.IsCancellationRequested) await Task.Delay(100, cancellationToken);
            _subscriptionService.MessageReceived -= handler;
            _logger.LogInformation("NewDatapointHandler: stopped");
        }

        private async void PublishLatestEntriesAsync(CancellationToken cancellationToken, int msInterval)
        {
            while (!_cancel && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var latestEntries = _latestEntryAggregator.ToList();
                    if (latestEntries.Count > 0)
                    {
                        var message = JsonConvert.SerializeObject(_latestEntryAggregator);
                        if (_latestEntryAggregator.Count() > 0)
                        {
                            _messagePublisher.PublishJSONMessage(message, MessagingQueues.LIVEDATA_MULTIPLENEWDATAPOINTS_QUEUE);
                            _logger.LogInformation($"NewDatapointHandler: published latest entries. Count: {_latestEntryAggregator.Count()}");
                            _latestEntryAggregator.Clear();
                        }
                    }
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
            return Task.CompletedTask;
        }
    }
}
