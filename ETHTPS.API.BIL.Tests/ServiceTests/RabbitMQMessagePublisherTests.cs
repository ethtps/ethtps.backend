using System.Text;

using ETHTPS.Configuration;
using ETHTPS.Configuration.Extensions;
using ETHTPS.Services.Infrastructure.Messaging;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ETHTPS.Tests.ServiceTests
{
    [TestFixture]
    public sealed class RabbitMQMessagePublisherTests : TestBase
    {
        private static string _host = "localhost";
        private static string _queue = "test_queue";
        private static IDBConfigurationProvider _configurationProvider;

        [SetUp]
        public void Setup()
        {
            _configurationProvider = ServiceProvider.GetRequiredService<IDBConfigurationProvider>();
            _host = _configurationProvider.GetFirstConfigurationString("RabbitMQ_Host_Dev") ?? "localhost";
        }

        [Test]
        public void PublishMessage_PublishesMessageToQueue()
        {
            var messagePublisher = new RabbitMQMessagePublisher(_configurationProvider);

            messagePublisher.PublishMessage("Hello, world!", _queue, _host);

            // Wait for the message to be received
            var receivedMessage = WaitForMessage();

            Assert.That(receivedMessage, Is.EqualTo("Hello, world!"));
        }

        [Test]
        public void PublishJSONMessage_PublishesSerializedObjectToQueue()
        {
            var messagePublisher = new RabbitMQMessagePublisher(_configurationProvider);
            Person obj = new() { Name = "John", Age = 30 };

            messagePublisher.PublishJSONMessage(obj, _queue, _host);

            // Wait for the message to be received
            var receivedMessage = WaitForMessage();

            // Deserialize the message and verify its contents
            var deserializedObj = JsonConvert.DeserializeObject<Person>(receivedMessage);
            Assert.That(deserializedObj, Is.Not.Null);
            Assert.That(deserializedObj.Name, Is.EqualTo("John"));
            Assert.That(deserializedObj.Age, Is.EqualTo(30));
        }

        private string WaitForMessage()
        {
            var factory = new ConnectionFactory() { HostName = _host };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Declare the queue if it does not already exist
            channel.QueueDeclare(queue: _queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            var receivedMessage = string.Empty;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                receivedMessage = Encoding.UTF8.GetString(body);
            };

            // Start consuming messages
            channel.BasicConsume(queue: _queue, autoAck: true, consumer: consumer);

            // Wait until a message is received
            while (string.IsNullOrEmpty(receivedMessage))
            {
                Thread.Sleep(100);
            }

            return receivedMessage;
        }

        public class Person
        {
            public required string Name { get; set; }
            public required int Age { get; set; }
        }
    }
}