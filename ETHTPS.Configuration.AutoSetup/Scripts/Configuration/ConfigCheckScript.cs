using ETHTPS.Configuration.AutoSetup.Infra;

namespace ETHTPS.Configuration.AutoSetup.Scripts.Configuration
{
    internal sealed class ConfigCheckScript : SetupScript
    {
        private const string _CONFIG_PROJECT_NAME = "ETHTPS.Configuration";

        public override void Pre()
        {
            Logger.Info("Checking configuration...");
        }

        public override void Run()
        {
            var solutionDir = Utils.TryGetSolutionDirectoryInfo();
            Assert.That(solutionDir != null, "Solution directory exists");
            Assert.File.AnyExists(Path.Combine(solutionDir?.FullName ?? string.Empty, ".env"),
                Path.Combine(solutionDir?.FullName ?? string.Empty, _CONFIG_PROJECT_NAME, ".env"));
            Assert.File.Exists(Path.Combine(solutionDir?.FullName ?? string.Empty, _CONFIG_PROJECT_NAME, "StartupConfig.json"));
            AddChild<ConfigDatabaseValidation>();
            base.Run();
        }

        public override void Post()
        {
            Logger.Ok("Configuration ok");
            base.Post();
        }
    }
}
