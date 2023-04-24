using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.API.Core.Services;
using ETHTPS.Configuration;
using ETHTPS.Configuration.Extensions;
using ETHTPS.Configuration.Validation.Exceptions;

using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis;

namespace ETHTPS.API.DependencyInjection
{
    public static class CachingExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services) =>
            services.AddSingleton<IConnectionMultiplexer>(
                x => ConnectionMultiplexer.Connect(
                    x.GetRequiredService<IDBConfigurationProvider>().GetFirstConfigurationString("RedisServer") ?? x.GetRequiredService<IDBConfigurationProvider>().GetFirstConfigurationString("RedisServerAlt")
                    ?? throw new ConfigurationStringNotFoundException(
                        "RedisServer/RedisServerAlt",
                        "Any")
                    )
                )
            .AddSingleton<ICachedDataService, RedisCachedDataService>();
    }
}
