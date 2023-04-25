using System.Text;

using ETHTPS.Services.Infrastructure.Messaging;

using RabbitMQ.Client;

namespace ETHTPS.Tests.ServiceTests.RabbitMQ
{
    [TestFixture]
    public class RabbitMQSubscriptionServiceTests
    {
        [Test]
        public void TestMessageReceivedEvent()
        {
            var config = new RabbitMQSubscriptionConfig
            {
                QueueName = "test-queue",
                Host = "localhost",
                Durable = false,
                Exclusive = false,
                AutoDelete = true,
                AutoAck = true,
                Arguments = null
            };

            var service = new RabbitMQSubscriptionService(config);

            bool eventRaised = false;
            string receivedMessage = null;

            service.MessageReceived += (sender, args) =>
            {
                eventRaised = true;
                receivedMessage = Encoding.UTF8.GetString(args.Body.ToArray());
            };

            var message = "test-message";
            var body = Encoding.UTF8.GetBytes(message);

            var factory = new ConnectionFactory() { HostName = config.Host };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.BasicPublish(exchange: "",
                                     routingKey: config.QueueName,
                                     basicProperties: null,
                                     body: body);
            }

            Assert.That(eventRaised, Is.True);
            Assert.That(receivedMessage, Is.EqualTo(message));
        }
    }
}
