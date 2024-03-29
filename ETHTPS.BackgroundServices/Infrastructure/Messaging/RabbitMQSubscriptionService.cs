﻿using System;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ETHTPS.Services.Infrastructure.Messaging
{
    public class RabbitMQSubscriptionService : IRabbitMQSubscriptionService
    {
        private readonly IConnection _connection;
        public IModel Channel { get; private set; }

        public RabbitMQSubscriptionService(RabbitMQSubscriptionConfig config)
        {
            var queueName = config.QueueName;

            var factory = new ConnectionFactory() { HostName = string.IsNullOrWhiteSpace(config.Host) ? RabbitMQSubscriptionConfig.DEFAULT_EXT_HOST_NAME : config.Host };
            _connection = factory.CreateConnection();
            Channel = _connection.CreateModel();
            Channel.QueueDeclare(queue: string.IsNullOrWhiteSpace(queueName) ? RabbitMQSubscriptionConfig.DEFAULT_QUEUE_NAME : queueName,
                                  durable: config.Durable,
                                  exclusive: config.Exclusive,
                                  autoDelete: config.AutoDelete,
                                  arguments: config.Arguments);
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (model, ea) =>
            {
                OnMessageReceived(ea);
            };
            consumer.Registered += (model, ea) =>
            {
                OnConsumerRegistered(ea);
            };
            consumer.Shutdown += (model, ea) =>
            {
                OnConsumerShutdown(ea);
            };
            consumer.Unregistered += (model, ea) =>
            {
                OnConsumerUnregistered(ea);
            };
            Channel.BasicConsume(queue: queueName,
                                  autoAck: config.AutoAck,
                                  consumer: consumer);
        }

        public event EventHandler<BasicDeliverEventArgs> MessageReceived;
        public event EventHandler<ConsumerEventArgs> Registered;
        public event EventHandler<ShutdownEventArgs> Shutdown;
        public event EventHandler<ConsumerEventArgs> Unregistered;

        protected virtual void OnMessageReceived(BasicDeliverEventArgs message)
        {
            MessageReceived?.Invoke(this, message);
        }

        protected virtual void OnConsumerRegistered(ConsumerEventArgs args)
        {
            Registered?.Invoke(this, args);
        }

        protected virtual void OnConsumerShutdown(ShutdownEventArgs args)
        {
            Shutdown?.Invoke(this, args);
        }

        protected virtual void OnConsumerUnregistered(ConsumerEventArgs args)
        {
            Unregistered?.Invoke(this, args);
        }

        public void Dispose()
        {
            Channel?.Dispose();
            _connection?.Dispose();
        }

        ~RabbitMQSubscriptionService()
        {
            Dispose();
        }
    }
}