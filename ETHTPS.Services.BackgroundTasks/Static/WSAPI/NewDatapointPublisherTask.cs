using System.Text;

using ETHTPS.Configuration;
using ETHTPS.Configuration.Extensions;
using ETHTPS.Data.Core.Models.LiveData.Triggers;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Services.BlockchainServices.HangfireLogging;
using ETHTPS.Services.Infrastructure.Messaging;

using Hangfire;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace ETHTPS.Services.BackgroundTasks.Static.WSAPI
{
    public sealed class NewDatapointPublisherTask : HangfireBackgroundService
    {
        protected override string ServiceName => "NewDatapointPublisherTask";
        private readonly RabbitMQSubscriptionService _subscriptionService;
        public NewDatapointPublisherTask(ILogger<HangfireBackgroundService> logger, EthtpsContext context, IDBConfigurationProvider configurationProvider) : base(logger, context)
        {
            _subscriptionService = new RabbitMQSubscriptionService(new RabbitMQSubscriptionConfig()
            {
                AutoAck = false,
                AutoDelete = false,
                QueueName = MessagingQueues.LIVEDATA_NEWDATAPOINT_QUEUE,
                Host = configurationProvider.GetFirstConfigurationString("RabbitMQ_Host_Dev")
            });
        }

        [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public override async Task RunAsync()
        {
            _logger.LogInformation("NewDatapointPublisherTask: listening...");
            _subscriptionService.MessageReceived += async (sender, e) =>
            {
                try
                {
                    var receivedMessage = Encoding.UTF8.GetString(e.Body.ToArray());

                    var datapoint = JsonConvert.DeserializeObject<L2DataUpdateModel>(receivedMessage);
                    _logger.LogInformation($"Received datapoint for {datapoint?.Provider}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing message from RabbitMQ");
                }
                finally
                {

                    _subscriptionService.Channel.BasicAck(e.DeliveryTag, true);
                }
            };
            while (true) await Task.Delay(1000);
        }
    }
}
