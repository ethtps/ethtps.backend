using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Utils.Database.Extensions
{
    public static class DatabaseInitializationExtensions
    {
        public static IServiceCollection AddDatabaseInitializationService(this IServiceCollection services) => services.AddScoped<IDatabaseInitializationService, DatabaseInitializationService>();
    }
}
