using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Utils.Database.Extensions
{
    public static class DatabaseInitializationExtensions
    {
        public static IServiceCollection AddDatabaseInitializer(this IServiceCollection services) => services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
    }
}
