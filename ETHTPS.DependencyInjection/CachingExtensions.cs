using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.API.Core.Services;
using ETHTPS.Configuration;
using ETHTPS.Configuration.Extensions;

using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis;

namespace ETHTPS.API.DependencyInjection
{
    public static class CachingExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services) =>
            services.AddSingleton<IConnectionMultiplexer>(
                x =>
                {
                    using (var scope = x.CreateScope())
                    {
                        return ConnectionMultiplexer.Connect(scope.ServiceProvider.GetService<IDBConfigurationProvider>()?.GetFirstConfigurationString("RedisServer") ?? scope.ServiceProvider.GetService<IDBConfigurationProvider>()?.GetFirstConfigurationString("RedisServerAlt")
                        ?? "localhost");
                    }
                })
            .AddSingleton<IRedisCacheService, RedisCachedDataService>();
    }
}
