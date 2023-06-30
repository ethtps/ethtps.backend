using ETHTPS.Configuration.AutoSetup.Infra.Exceptions;

namespace ETHTPS.Configuration.AutoSetup;

/// <summary>
/// A class for making setup assertions.
/// </summary>
internal static class Assert
{
    public static void That(bool condition)
    {
        if (!condition)
        {
            throw new AutoSetupException();
        }
    }

    public static void That(bool condition, string details)
    {
        if (!condition)
        {
            var m = "Condition not met: " + details;
            Logger.Error(m);
            throw new AutoSetupException(m);
        }
        Logger.Info($"Ok: {details}");
    }

    public static void That(Func<bool> condition)
    {
        That(condition());
    }

    public static void That(Func<bool> condition, string details)
    {
        That(condition(), details);
    }

    public static class File
    {
        public static void Exists(string path)
        {
            Assert.That(System.IO.File.Exists(path), $"File {path} exists");
        }
    }
}