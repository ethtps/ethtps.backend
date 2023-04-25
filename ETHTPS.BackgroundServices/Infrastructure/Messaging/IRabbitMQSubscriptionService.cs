using System;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ETHTPS.Services.Infrastructure.Messaging
{
    public interface IRabbitMQSubscriptionService : IDisposable
    {
        event EventHandler<BasicDeliverEventArgs> MessageReceived;
        event EventHandler<ConsumerEventArgs> Registered;
        event EventHandler<ShutdownEventArgs> Shutdown;
        event EventHandler<ConsumerEventArgs> Unregistered;
        IModel Channel { get; }
    }
}
