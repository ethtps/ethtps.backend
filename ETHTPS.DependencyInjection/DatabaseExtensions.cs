using ETHTPS.Configuration;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.API.DependencyInjection
{
    public static class DatabaseExtensions
    {
        public static string GetDefaultConnectionString(this DBConfigurationProviderWithCache provider, Microservice microservice, string connectionStringName)
        {
            var strings = provider.GetConfigurationStringsForMicroservice(microservice.GetFullName());
            var result = strings?
                             .FirstOrDefault(x =>
                                 x.Name == connectionStringName)?.Value
                         ??
#if DEVELOPMENT
                         throw new ArgumentException($"Couldn't find a connection string called \"{connectionStringName}\" for {microservice} for \"{Constants.ENVIRONMENT}\" environment\r\nDetails: {JsonConvert.SerializeObject(strings)}");
#else
                            throw new ArgumentException($"Couldn't find a connection string called \"{connectionStringName}\" for {microservice} for \"{Configuration.Constants.ENVIRONMENT}\" environment");
#endif
            return result;

        }

        public static string GetDefaultConnectionString(this DBConfigurationProvider provider, Microservice microservice, string connectionStringName)
        {
            var strings = provider.GetConfigurationStringsForMicroservice(microservice.GetFullName());
            var result = strings?
                             .FirstOrDefault(x =>
                                                                 x.Name == connectionStringName)?.Value
                         ??
#if DEVELOPMENT
                         throw new ArgumentException($"Couldn't find a connection string called \"{connectionStringName}\" for {microservice} for \"{Constants.ENVIRONMENT}\" environment\r\nDetails: {JsonConvert.SerializeObject(strings)}");
#else
                            throw new ArgumentException($"Couldn't find a connection string called \"{connectionStringName}\" for {microservice} for \"{Configuration.Constants.ENVIRONMENT}\" environment");
#endif
            return result;

        }

        public static string GetDefaultConnectionString(this IServiceCollection services, Microservice microservice) => services.GetConnectionString(microservice, "ConnectionString");

        public static string GetConnectionString(this IServiceCollection services, Microservice microservice, string connectionStringName)
        {
            using var built = services.BuildServiceProvider();
            var provider = built.GetRequiredService<DBConfigurationProviderWithCache>();
            var strings = provider.GetConfigurationStringsForMicroservice(microservice.GetFullName());
            var result = GetDefaultConnectionString(provider, microservice, connectionStringName);
            return result;
        }

        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, Microservice microservice)
        {
            try
            {
                var configurationString = services.GetDefaultConnectionString(microservice);
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
