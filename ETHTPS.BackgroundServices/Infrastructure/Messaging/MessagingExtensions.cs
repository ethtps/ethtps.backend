using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Services.Infrastructure.Messaging
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddRabbitMQMessagePublisher(this IServiceCollection services) => services.AddTransient<IMessagePublisher, RabbitMQMessagePublisher>();
    }
}
