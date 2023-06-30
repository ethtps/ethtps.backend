using ETHTPS.Configuration.AutoSetup.Infra;
using ETHTPS.Configuration.AutoSetup.Scripts.Configuration;

namespace ETHTPS.Configuration.AutoSetup.Scripts;

/// <summary>
/// Configures the system if necessary. The application is guaranteed to run properly after this script is run.
/// </summary>
public sealed class ETHTPSAutoSetupScript
{
    private readonly SetupScript _script;
    public ETHTPSAutoSetupScript(string environmentName)
    {
        var builder = new SetupScriptBuilder(environmentName);
        builder.AddPre(() => Logger.Info("Checking system configuration..."));
        builder.Add(new FrameworkCheckScript());
        builder.Add(new ConfigCheckScript());
        builder.AddPost(() => Logger.Info("System configuration check completed"));
        _script = builder.Build();
    }

    public void Run()
    {
        _script.Run();
    }
}