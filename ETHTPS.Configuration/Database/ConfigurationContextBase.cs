﻿using ETHTPS.Data.Core.Database;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ETHTPS.Configuration.Database
{
    public abstract class ConfigurationContextBase : ContextBase<ConfigurationContext>
    {
        public ConfigurationContextBase()
        {
            Database.SetCommandTimeout(TimeSpan.FromSeconds(10));
        }

        public ConfigurationContextBase(DbContextOptions<ConfigurationContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromSeconds(60));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies().EnableThreadSafetyChecks();
        }

        public async Task<int> InsertOrUpdateConfigurationStringAsync(
            string microserviceName,
            string environmentName,
            string configStringName,
            string configStringValue)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[Configuration].[InsertOrUpdateConfigurationString]";

                command.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter("@MicroserviceName", microserviceName),
                new SqlParameter("@EnvironmentName", environmentName),
                new SqlParameter("@ConfigStringName", configStringName),
                new SqlParameter("@ConfigStringValue", configStringValue)
                });

                await Database.OpenConnectionAsync();
                var result = await command.ExecuteNonQueryAsync();
                return result;
            }
        }

        public async Task InsertOrUpdateProviderConfigurationStringAsync(string providerName, string configurationStringName, string configurationStringValue, string environmentName)
        {
            var providerNameParam = new SqlParameter("@ProviderName", providerName);
            var configurationStringNameParam = new SqlParameter("@ConfigurationStringName", configurationStringName);
            var configurationStringValueParam = new SqlParameter("@ConfigurationStringValue", configurationStringValue);
            var environmentNameParam = new SqlParameter("@EnvironmentName", environmentName);

            await Database.ExecuteSqlRawAsync("EXEC [Configuration].[InsertOrUpdateProviderConfigurationString] @ProviderName, @ConfigurationStringName, @ConfigurationStringValue, @EnvironmentName",
                providerNameParam, configurationStringNameParam, configurationStringValueParam, environmentNameParam);
        }
    }
}
