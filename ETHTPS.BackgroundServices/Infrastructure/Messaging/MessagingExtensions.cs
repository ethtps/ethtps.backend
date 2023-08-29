using System.Collections.Generic;

using ETHTPS.Configuration;
using ETHTPS.Configuration.Extensions;

using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Services.Infrastructure.Messaging
{
    public static class MessagingExtensions
    {
        public enum AllowedSubscriptionScope { BlockDataAggregator }
        private static Dictionary<AllowedSubscriptionScope, string> _queueCorrespondence = new()
        {
            { AllowedSubscriptionScope.BlockDataAggregator, "L2DataRequestQueue" }
        };
        public static IServiceCollection AddRabbitMQMessagePublisher(this IServiceCollection services) => services.AddTransient<IMessagePublisher, RabbitMQMessagePublisher>();
        public static IServiceCollection AddRabbitMQSubscriptionService(this IServiceCollection services, AllowedSubscriptionScope s)
        =>
            services.AddSingleton<IRabbitMQSubscriptionService>(x =>
            {
                using (var scope = x.CreateScope())
                    return new RabbitMQSubscriptionService(new RabbitMQSubscriptionConfig()
                    {
                        AutoAck = false,
                        QueueName = _queueCorrespondence[s],
                        Host = scope.ServiceProvider.GetRequiredService<DBConfigurationProviderWithCache>().GetFirstConfigurationString("RabbitMQ_Host_Dev")
                    });
            });

    }
}
