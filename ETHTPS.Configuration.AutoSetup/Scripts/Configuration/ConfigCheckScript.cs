using ETHTPS.Configuration.AutoSetup.Infra;

namespace ETHTPS.Configuration.AutoSetup.Scripts.Configuration
{
    internal sealed class ConfigCheckScript : SetupScript
    {
        public override void Pre()
        {
            Logger.Info("Checking configuration...");
        }

        public override void Run()
        {
            Assert.File.Exists(".env");
        }

        public override void Post()
        {
            Logger.Info("Configuration ok");
        }

        public override void Clean()
        {

        }
    }
}
