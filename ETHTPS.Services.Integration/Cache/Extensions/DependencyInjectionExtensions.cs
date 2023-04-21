using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Services.Integration.Cache.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services) => services.AddScoped<ICacheManager, RedisCacheManager>();
    }
}
