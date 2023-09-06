using System.Text;

using ETHTPS.Configuration;
using ETHTPS.Configuration.Extensions;

using Microsoft.Extensions.DependencyInjection;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace ETHTPS.Tests.PerformanceTests
{
    namespace RabbitMQ.Client.Unit
    {
        [TestFixture]
        [Category("Performance")]
        public class ThroughputTest : TestBase
        {
            private IConnection? _conn;
            private IModel? _ch;
            private EventingBasicConsumer? _consumer;

            [SetUp]
            public void Init()
            {
                ConnectionFactory? cf = new()
                {
                    AutomaticRecoveryEnabled = false,
                    HostName = ServiceProvider.GetRequiredService<DBConfigurationProviderWithCache>().GetFirstConfigurationString("RabbitMQ_Host_Dev") ?? "localhost"
                };
                _conn = cf.CreateConnection();
                _ch = _conn.CreateModel();
            }
            [TearDown]
            public void Cleanup()
            {
                _conn?.Close();
            }
            [Test]
            public void TestThroughput()
            {
                string q = _ch.QueueDeclare();
                _consumer = new EventingBasicConsumer(_ch);
                uint msgCount = 1000000;
                uint ackCount = 0;
                uint ackLimit = 1000;
                byte[] data = Encoding.UTF8.GetBytes("hello");
                if (_ch == null)
                {
                    throw new NullReferenceException("Channel is null");
                }
                IBasicProperties? props = _ch.CreateBasicProperties();
                props.ContentType = "text/plain";
                props.DeliveryMode = 2; // persistent 
                props.ContentEncoding = "UTF-8";
                props.Headers = new Dictionary<string, object>
                {
                    { "foo", "bar" },
                    { "baz", "qux" }
                };
                props.Expiration = "1000";
                props.MessageId = "message-id";
                props.Timestamp = new AmqpTimestamp(DateTime.Now.Ticks);
                props.Type = "type";
                props.UserId = "user-id";
                props.AppId = "app-id";
                props.ClusterId = "cluster-id";
                ManualResetEvent? done = new ManualResetEvent(false);
                Thread? t = new(delegate ()
                {
                    _consumer.Received += (sender, e) =>
                    {
                        while (true)
                        {
                            if (e == null)
                            {
                                break;
                            }
                            ackCount++;
                            if (ackCount == msgCount)
                            {
                                done.Set();
                                break;
                            }
                            else if (ackCount % ackLimit == 0)
                            {
                                _ch.BasicAck(e.DeliveryTag, false);
                            }
                        }
                    };
                });
                t.Start();
                _ch.BasicConsume(q, true, _consumer);
                _ch.BasicQos(0, 100, false);
                for (uint i = 0; i < msgCount; i++)
                {
                    _ch.BasicPublish("", q, false, props, data);
                }
                done.WaitOne();
                Assert.That(ackCount, Is.EqualTo(msgCount));
            }
        }
    }
}
