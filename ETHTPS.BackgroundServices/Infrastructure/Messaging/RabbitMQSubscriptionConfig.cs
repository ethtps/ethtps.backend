using System.Collections.Generic;

namespace ETHTPS.Services.Infrastructure.Messaging
{
    public sealed class RabbitMQSubscriptionConfig
    {
        public string QueueName { get; set; } = "default";
        public string Host { get; set; } = "localhost";
        public bool Durable { get; set; } = false;
        public bool Exclusive { get; set; } = false;
        public bool AutoDelete { get; set; } = false;
        public bool AutoAck { get; set; } = true;
        public IDictionary<string, object> Arguments { get; set; } = null;
    }
}
