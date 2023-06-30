namespace ETHTPS.Configuration.AutoSetup.Infra.Exceptions.Scripts;

public sealed class PreException : SetupScriptException
{
    public PreException()
    {

    }

    public PreException(Exception innerException) : base(innerException)
    {

    }

    public PreException(string actionName) : base($"Script failed at {actionName} pre-step")
    {

    }

    public PreException(string actionName, Exception innerException) : base($"Script failed at {actionName} pre-step", innerException)
    {

    }
}