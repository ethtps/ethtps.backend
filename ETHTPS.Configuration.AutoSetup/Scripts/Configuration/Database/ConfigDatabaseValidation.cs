using ETHTPS.Configuration.AutoSetup.Infra;
using ETHTPS.Configuration.Database;

using Microsoft.EntityFrameworkCore;

namespace ETHTPS.Configuration.AutoSetup.Scripts.Configuration.Database
{
    [Obsolete("Use the other validation script that includes the main database instead", true)]
    internal sealed class ConfigDatabaseValidation : SetupScript
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
                connectionString = API.DependencyInjection.CoreServicesExtensions.GetConfigurationServiceConnectionString(), "Get configuration service connection string");
            var builder = new SetupScriptBuilder();
            var options = new DbContextOptionsBuilder<ConfigurationContext>();
            options.UseSqlServer(connectionString);
            var context = new ConfigurationContext(options.Options);
            builder.Add(() =>
            {
                if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
                Assert.That(context.Database.CanConnect);
            }, "Can connect to config database");
            builder.Add(new SchemaInitializer<ConfigurationContext>(context, "Configuration"), "ConfigurationContext schema init");
            builder.Add(new TableInitializer<ConfigurationContext>(context, "Configuration"), "ConfigurationContext table init");
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
