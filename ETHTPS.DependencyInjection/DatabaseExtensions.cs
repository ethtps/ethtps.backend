using ETHTPS.Configuration;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

namespace ETHTPS.API.DependencyInjection
{
    public static class DatabaseExtensions
    {
        public static string GetDefaultConnectionString(this IServiceCollection services, string appName) => services.GetConnectionString(appName, "ConnectionString");
        public static string GetConnectionString(this IServiceCollection services, string appName, string connectionStringName)
        {
            using var built = services.BuildServiceProvider();
            var provider = built.GetRequiredService<DBConfigurationProviderWithCache>();
            var strings = provider.GetConfigurationStringsForMicroservice(appName);
            var result = strings?
                             .FirstOrDefault(x =>
                                 x.Name == connectionStringName)?.Value
                         ??
#if DEBUG
                         throw new ArgumentException($"Couldn't find a connection string called \"{connectionStringName}\" for {appName} for \"{Constants.ENVIRONMENT}\" environment\r\nDetails: {JsonConvert.SerializeObject(strings)}");
#else
                            throw new ArgumentException($"Couldn't find a connection string called \"{connectionStringName}\" for {appName} for \"{Constants.ENVIRONMENT}\" environment");
#endif
            return result;
        }

        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, string appName)
        {
            try
            {
                var configurationString = services.GetDefaultConnectionString(appName);
                services.AddDbContext<EthtpsContext>(options => options.UseSqlServer(configurationString), ServiceLifetime.Scoped);
                return services;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Not allowed", ex);
            }

        }
    }
}
