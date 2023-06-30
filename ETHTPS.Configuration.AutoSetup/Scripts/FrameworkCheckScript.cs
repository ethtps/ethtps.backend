using ETHTPS.Configuration.AutoSetup.Infra;

namespace ETHTPS.Configuration.AutoSetup.Scripts
{
    internal sealed class FrameworkCheckScript : SetupScript
    {
        public override void Pre()
        {
            Logger.Info("Running .Pre()");
        }

        public override void Run()
        {
            Logger.Info("Doing framework check...");
            Logger.Info(">begin script<");
            Logger.Debug("This is a debug message");
            Logger.Info("This is an info message");
            Logger.Warn("This is a warning message");
            Logger.Error("This is an error message");
            Assert.That(true);
            Logger.Info(">end script<");
        }

        public override void Post()
        {
            Logger.Info("Running .Post()");
        }

        public override void Clean()
        {
            Logger.Info("Cleaning up...");
        }
    }
}
