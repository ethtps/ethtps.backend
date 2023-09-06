namespace ETHTPS.Configuration.AutoSetup.Infra.Exceptions;

/// <summary>
/// Represents an exception that occurs during the auto setup process.
/// </summary>
public sealed class AutoSetupException : Exception
{
    public AutoSetupException()
    {

    }

    public AutoSetupException(string message) : base(message)
    {

    }

    public AutoSetupException(string message, Exception innerException) : base(message, innerException)
    {

    }
}