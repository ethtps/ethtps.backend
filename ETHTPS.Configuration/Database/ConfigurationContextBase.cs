using ETHTPS.Data.Core.Database;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ETHTPS.Configuration.Database
{
    public abstract class ConfigurationContextBase : ContextBase<ConfigurationContext>
    {
        protected ConfigurationContextBase()
        {
            Database.SetCommandTimeout(TimeSpan.FromSeconds(10));
        }

        protected ConfigurationContextBase(DbContextOptions<ConfigurationContext> options)
            : base(options)
        {
            Database.AutoTransactionBehavior = AutoTransactionBehavior.WhenNeeded;
            Database.SetCommandTimeout(TimeSpan.FromSeconds(10));
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

        public List<ConfigurationString> GetConfigurationStrings(string providerName, string environmentName)
        {
            return Set<ConfigurationString>()
                .FromSqlRaw("EXECUTE Configuration.GetConfigurationStringsByProviderAndEnvironment @ProviderName, @EnvironmentName",
                    new SqlParameter("@ProviderName", providerName),
                    new SqlParameter("@EnvironmentName", environmentName))
                .ToList();
        }

        public List<ConfigurationString> GetConfigurationStringsOfMicroservice(string microserviceName, string environmentName)
        {
            var parameters = new object[]
            {

                new SqlParameter("@MicroserviceName", microserviceName),
                new SqlParameter("@EnvironmentName", environmentName)
            };
            return Set<ConfigurationString>()
                .FromSqlRaw("EXEC [Configuration].[GetConfigurationStringsOfMicroservice] @MicroserviceName, @EnvironmentName", parameters)
                .AsEnumerable().Select(x => new ConfigurationString()
                {
                    Name = x.Name,
                    Value = x.Value,
                    IsSecret = x.IsSecret,
                    IsEncrypted = x.IsEncrypted,
                    EncryptionAlgorithmOrHint = x.EncryptionAlgorithmOrHint
                }).ToList();
        }

        public IEnumerable<InsertGenerationResult> GenerateInsertsForSchema(string schema)
        {
            var result = Enumerable.Empty<InsertGenerationResult>();
            var connection = (SqlConnection)Database.GetDbConnection();
            connection.InfoMessage += (sender, e) =>
            {
                Console.WriteLine(e.Message);
            };
            try
            {
                connection.Open();
                var schemaParam = new SqlParameter("@schemaName", schema);
                result = Set<InsertGenerationResult>().FromSqlRaw("EXEC GenerateInsertsForSchema_2 @schemaName", schemaParam).ToArray();
            }
            finally
            {
                connection.Close();
            }
            return result;
        }
    }
}
