using ETHTPS.API.Core.Services;
using ETHTPS.Configuration;
using ETHTPS.Configuration.Database;
using ETHTPS.Core;
using ETHTPS.Data.Core.Extensions;

using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis;

namespace ETHTPS.API.DependencyInjection
{
    internal static class CachingExtensions
    {
        /// <summary>
        /// Sets up Redis caching for this application. Configuration is automatic and based on an <see cref="DBConfigurationProviderWithCache"/>.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        internal static IServiceCollection AddRedisCache(this IServiceCollection services) =>
            services.AddSingleton<IConnectionMultiplexer>(
                x =>
                {
                    using var scope = x.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();
                    return ConnectionMultiplexer.Connect((context.ConfigurationStrings
                        .FirstIfAny(z => z.Name == "RedisServer") ?? context.ConfigurationStrings.FirstIfAny(z => z.Name == "RedisServerAlt")).Value);
                })
            .AddSingleton<IRedisCacheService, RedisCachedDataService>();
    }
}
