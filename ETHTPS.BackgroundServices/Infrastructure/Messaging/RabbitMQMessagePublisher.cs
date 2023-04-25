using System.Text;

using ETHTPS.Configuration;
using ETHTPS.Configuration.Extensions;

using Newtonsoft.Json;

using RabbitMQ.Client;

namespace ETHTPS.Services.Infrastructure.Messaging
{
    public sealed class RabbitMQMessagePublisher : IMessagePublisher
    {
        private readonly string? _host;

        public RabbitMQMessagePublisher(IDBConfigurationProvider configurationProvider)
        {
            _host = configurationProvider.GetFirstConfigurationString("RabbitMQ_Host_Dev");
        }

        public void PublishMessage(string message, string queue) => PublishMessage(message, queue, _host);

        public void PublishJSONMessage<T>(T message, string queue, string host) => PublishMessage(JsonConvert.SerializeObject(message), queue, host ?? _host);

        public void PublishJSONMessage<T>(T message, string queue) => PublishJSONMessage(message, queue, _host ?? "localhost");

        public void PublishMessage(string message, string queue, string host)
        {
            var factory = new ConnectionFactory() { HostName = _host ?? host };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue,
                                     false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                                     routingKey: queue,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
