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
        public class ThroughputTest : TestBase
        {
            private IConnection conn;
            private IModel ch;
            private EventingBasicConsumer consumer;
            [SetUp]
            public void Init()
            {
                ConnectionFactory cf = new ConnectionFactory();
                cf.AutomaticRecoveryEnabled = false;
                cf.HostName = ServiceProvider.GetRequiredService<IDBConfigurationProvider>().GetFirstConfigurationString("RabbitMQ_Host_Dev") ?? "localhost";
                conn = cf.CreateConnection();
                ch = conn.CreateModel();
            }
            [TearDown]
            public void Cleanup()
            {
                conn.Close();
            }
            [Test]
            public void TestThroughput()
            {
                string q = ch.QueueDeclare();
                consumer = new EventingBasicConsumer(ch);
                uint msgCount = 1000000;
                uint ackCount = 0;
                uint ackLimit = 1000;
                byte[] data = Encoding.UTF8.GetBytes("hello");
                IBasicProperties props = ch.CreateBasicProperties();
                props.ContentType = "text/plain";
                props.DeliveryMode = 2; // persistent 
                props.ContentEncoding = "UTF-8";
                props.Headers = new Dictionary<string, object>();
                props.Headers.Add("foo", "bar");
                props.Headers.Add("baz", "qux");
                props.Expiration = "1000";
                props.MessageId = "message-id";
                props.Timestamp = new AmqpTimestamp(DateTime.Now.Ticks);
                props.Type = "type";
                props.UserId = "user-id";
                props.AppId = "app-id";
                props.ClusterId = "cluster-id";
                ManualResetEvent done = new ManualResetEvent(false);
                Thread t = new Thread(delegate ()
                {
                    consumer.Received += (sender, e) =>
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
                                ch.BasicAck(e.DeliveryTag, false);
                            }
                        }
                    };
                });
                t.Start();
                ch.BasicConsume(q, true, consumer);
                ch.BasicQos(0, 100, false);
                for (uint i = 0; i < msgCount; i++)
                {
                    ch.BasicPublish("", q, false, props, data);
                }
                done.WaitOne();
                Assert.That(ackCount, Is.EqualTo(msgCount));
            }
        }
    }
}
