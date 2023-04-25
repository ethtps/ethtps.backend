using System.Text;

using ETHTPS.Configuration;
using ETHTPS.Configuration.Extensions;

using Newtonsoft.Json;

using RabbitMQ.Client;

namespace ETHTPS.Services.Infrastructure.Messaging
{
    public class RabbitMQMessagePublisher : IMessagePublisher
    {
        private readonly string? _host;

        public RabbitMQMessagePublisher(IDBConfigurationProvider configurationProvider)
        {
            _host = configurationProvider.GetFirstConfigurationString("RabbitMQ_Host_Dev");
        }

        public void PublishJSONMessage<T>(T message, string queue, string? host) => PublishMessage(JsonConvert.SerializeObject(message), queue, host ?? _host);

        public void PublishMessage(string message, string queue, string host = "localhost")
        {
            var factory = new ConnectionFactory() { HostName = _host ?? host };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue,
                                     durable: false,
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
