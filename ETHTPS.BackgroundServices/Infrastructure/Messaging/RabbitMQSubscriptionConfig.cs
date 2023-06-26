using System.Collections.Generic;

namespace ETHTPS.Services.Infrastructure.Messaging
{
    public sealed class RabbitMQSubscriptionConfig
    {
        public const string DEFAULT_QUEUE_NAME = "default";
        public const string DEFAULT_HOST_NAME = "localhost";
        public string QueueName { get; set; } = DEFAULT_QUEUE_NAME;
        public string Host { get; set; } = DEFAULT_HOST_NAME;
        public bool Durable { get; set; } = false;
        public bool Exclusive { get; set; } = false;
        public bool AutoDelete { get; set; } = false;
        public bool AutoAck { get; set; } = true;
        public IDictionary<string, object> Arguments { get; set; } = null;
    }
}
