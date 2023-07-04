using ETHTPS.API.DependencyInjection;
using ETHTPS.Configuration.AutoSetup.Infra;
using ETHTPS.Configuration.Database;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace ETHTPS.Configuration.AutoSetup.Scripts.Configuration.Database
{
    internal sealed class ETHTPSDatabaseValidation : SetupScript
    {
        private const string _CONFIG_PROJECT_NAME = "ETHTPS.Configuration";

        public override void Pre()
        {
            Logger.Info("Checking configuration provider database...");
        }

        public override void Run()
        {
            string? connectionString = null;
            Assert.DoesNotThrow(() =>
                connectionString = CoreServicesExtensions.GetConfigurationServiceConnectionString(), "Get configuration service connection string");
            var builder = new SetupScriptBuilder();
            var options = new DbContextOptionsBuilder<ConfigurationContext>();
            options.UseSqlServer(connectionString);
            var context = new ConfigurationContext(options.Options);
            var configurationProvider = new DBConfigurationProvider(context, null);
            var ethtpsOptions = new DbContextOptionsBuilder<EthtpsContext>();
            ethtpsOptions.UseSqlServer(
                configurationProvider.GetDefaultConnectionString(_CONFIG_PROJECT_NAME, "ConnectionString"));
            builder.Add(new SchemaInitializer<ConfigurationContext>(context));
            var schemaFileName = Path.Combine(Utils.SqlDirectoryPath, "schemas.json");
            Assert.File.Exists(schemaFileName);
            var allSchemas = JsonConvert.DeserializeObject<string[]>(File.ReadAllText(schemaFileName))?.ToList();
            allSchemas?.ForEach(s => builder.Add(new SchemaInitializer<EthtpsContext>(new EthtpsContext(ethtpsOptions.Options), s), $"Initialize [{s}].[*]"));
            allSchemas?.ForEach(s => builder.Add(new TableInitializer<ConfigurationContext>(context, s), $"Initialize [{s}].[*]"));

            AddChild(builder.Build());
            base.Run();
        }

        public override void Post()
        {
            Logger.Ok("Configuration provider database ok");
            base.Post();
        }
    }
}
