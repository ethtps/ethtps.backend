namespace ETHTPS.Configuration.AutoSetup.Infra.Exceptions.Scripts;

public class SetupScriptException : Exception
{
    public SetupScriptException()
    {

    }

    public SetupScriptException(Exception innException) : base("No details provided for this exception", innException)
    {

    }

    public SetupScriptException(string message) : base(message)
    {

    }

    public SetupScriptException(string message, Exception innerException) : base(message, innerException)
    {

    }
}