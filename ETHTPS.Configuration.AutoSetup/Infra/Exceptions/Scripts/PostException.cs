namespace ETHTPS.Configuration.AutoSetup.Infra.Exceptions.Scripts;

public sealed class PostException : SetupScriptException
{
    public PostException()
    {

    }
    public PostException(Exception innerException) : base(innerException)
    {

    }
    public PostException(string actionName) : base($"Script failed at {actionName} post-step")
    {

    }

    public PostException(string actionName, Exception innerException) : base($"Script failed at {actionName} post-step", innerException)
    {

    }
}