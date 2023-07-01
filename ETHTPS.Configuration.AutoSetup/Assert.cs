using ETHTPS.Configuration.AutoSetup.Infra.Exceptions;

namespace ETHTPS.Configuration.AutoSetup;

/// <summary>
/// A class for making setup assertions.
/// </summary>
internal static class Assert
{
    /// <summary>
    /// Runs an action and if necessary, swallows and then logs any potential exception. 
    /// </summary>
    /// <param name="action"></param>
    /// <param name="details">A very short summary describing this operation</param>
    /// <returns>A value indicating whether an exception was swallowed.</returns>
    public static bool Controlled(Action action, string? details = null)
    {
        try
        {
            action();
            return false;
        }
        catch (Exception ex)
        {
            var m = $"{(string.IsNullOrWhiteSpace(details) ? string.Empty : ($@"{details}:\w"))}Swallowed exception of type {ex.GetType().Name}";
            Logger.Warn(m);
            return true;
        }
    }
    public static void That(bool condition)
    {
        if (!condition)
        {
            throw new AutoSetupException(nameof(condition));
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
        Logger.Ok($"{details}");
    }

    public static void That(Func<bool> condition)
    {
        That(condition());
    }

    public static void That(Func<bool> condition, string details)
    {
        That(condition(), details);
    }

    public static void DoesNotThrow(Action action, string details)
    {
        try
        {
            action();
            Logger.Ok(details);
        }
        catch (Exception e)
        {
            var m = $"{details} {e.GetType().Name}";
            Logger.Error(m);
            throw new AutoSetupException(m, e);
        }
    }
    public static void DoesNotThrow(Action action) => DoesNotThrow(action, $"Expected action to not throw an exception");

    public static class File
    {
        public static void Exists(string path)
        {
            Assert.That(System.IO.File.Exists(path), $"File {path} exists");
        }

        public static void AnyExists(params string[] paths)
        {
            var result = paths.Select(System.IO.File.Exists).Any();
            Assert.That(result, $"Any of the files [{string.Join(", ", paths)}] exist(s)");
        }
    }

    public static class Directory
    {
        public static void Exists(string? path)
        {
            Assert.That(System.IO.Directory.Exists(path), $"Directory {path} exists");
        }
    }
}