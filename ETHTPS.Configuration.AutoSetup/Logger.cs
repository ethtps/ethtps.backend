namespace ETHTPS.Configuration.AutoSetup;

/// <summary>
/// Basic logger class that doesn't use any external dependencies.
/// </summary>
internal static class Logger
{
    public static bool LogToFile { get; set; } = true;

    public static string LogFileName { get; set; } = "AutoSetup.log";

    /// <summary>
    /// Logs a message with the given log level.
    /// </summary>
    private static void Log(string level, string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        string output = $"{level}: {DateTime.Now}: {message}";
        Console.WriteLine(output);
        Console.ResetColor();

        if (LogToFile)
        {
            try
            {
                File.AppendAllText(LogFileName, output + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: {DateTime.Now}: Failed to log message to file: {ex.Message}");
                Console.ResetColor();
            }
        }
    }

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    public static void Info(string message)
    {
        Log("INFO", message, ConsoleColor.White);
    }

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    public static void Warn(string message)
    {
        Log("WARN", message, ConsoleColor.Yellow);
    }

    /// <summary>
    /// Logs an error message.
    /// </summary>
    public static void Error(string message)
    {
        Log("ERROR", message, ConsoleColor.Red);
    }

    /// <summary>
    /// Logs a debug message.
    /// </summary>
    public static void Debug(string message)
    {
        Log("DEBUG", message, ConsoleColor.Green);
    }
}
