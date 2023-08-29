using System.Diagnostics;
using System.Text;

using ETHTPS.Data.Core.Models.LiveData.Triggers;
using ETHTPS.Services;
using ETHTPS.Services.Infrastructure.Messaging;
using ETHTPS.Services.LiveData;

using Newtonsoft.Json;

using RabbitMQ.Client.Events;

namespace ETHTPS.TaskRunner.BackgroundServices
{
    /// <summary>
    /// Handles new datapoints received from RabbitMQ, aggregates them by keeping only the latest entry and publishes them to connected clients through WSAPI by publishing the aggregated data to another queue (it can be done directly but this approach might be useful should we ever need to scale up the WSAPI service).
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Hosting.IHostedService" />
    public sealed class NewDatapointHandler : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly LatestEntryAggregator<string, L2DataUpdateModel> _latestEntryAggregator = new LatestEntryAggregator<string, L2DataUpdateModel>();
        private bool _cancel = false;

        public NewDatapointHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _subscriptionService = scope.ServiceProvider.GetRequiredService<IRabbitMQSubscriptionService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<NewDatapointHandler>>();
            logger.LogInformation("NewDatapointHandler: listening...");
            PublishLatestEntriesAsync(cancellationToken, 2500, scope.ServiceProvider.GetRequiredService<IMessagePublisher>(), logger);
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
                    logger.LogError(ex, "Error processing message from RabbitMQ");
                }
                finally
                {
                    _subscriptionService.Channel.BasicAck(e.DeliveryTag, true);
                    stopwatch.Stop();
                    logger.LogInformation($"NewDatapointHandler: processed message in {stopwatch.ElapsedMilliseconds}ms");
                }
            };
            _subscriptionService.MessageReceived += handler;
            while (!_cancel && !cancellationToken.IsCancellationRequested) await Task.Delay(100, cancellationToken);
            _subscriptionService.MessageReceived -= handler;
            logger.LogInformation("NewDatapointHandler: stopped");
        }

        private async void PublishLatestEntriesAsync(CancellationToken cancellationToken, int msInterval, IMessagePublisher messagePublisher, ILogger<NewDatapointHandler> logger)
        {
            while (!_cancel && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var latestEntries = _latestEntryAggregator.ToList();
                    if (latestEntries.Count > 0)
                    {
                        if (_latestEntryAggregator.Count() > 0)
                        {
                            messagePublisher.PublishJSONMessage(_latestEntryAggregator.ToArray(), MessagingQueues.LIVEDATA_MULTIPLENEWDATAPOINTS_QUEUE);
                            logger.LogInformation($"NewDatapointHandler: published latest entries. Count: {_latestEntryAggregator.Count()}");
                            _latestEntryAggregator.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error publishing latest entries");
                }
                await Task.Delay(msInterval, cancellationToken);
            }
            logger.LogInformation("Stopped aggregator publisher task");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancel = true;
            return Task.CompletedTask;
        }
    }
}
