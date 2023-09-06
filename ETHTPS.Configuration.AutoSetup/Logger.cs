namespace ETHTPS.Configuration.AutoSetup;

/// <summary>
/// Basic logger class that doesn't use any external dependencies.
/// </summary>
internal static class Logger
{
    static Logger()
    {
#if !RELEASE
        if (File.Exists(LogFileName)) File.Delete(LogFileName);
#endif
        Info($"Logger initialization...");
        Info($"Current time: {DateTime.Now}");
    }

    public static bool LogToFile { get; set; } = true;

    public static string LogFileName { get; set; } = "AutoSetup.log";

    public static class LeftPadding
    {
        internal static int LeftPad { get; set; } = 0;
        private static readonly int _incrementAmount = 1;
        public static char PaddingCharacter { get; set; } = '\t';

        public static void Increment() => LeftPad += _incrementAmount;

        public static void Decrement()
        {
            if (LeftPad > 0) LeftPad -= _incrementAmount;
        }
    }

    /// <summary>
    /// Logs a message with the given log level.
    /// </summary>
    private static void Log(string level, string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        string li = LeftPadding.LeftPad > 0 ? "" : string.Empty;
        string output = $"{li}{string.Empty.PadLeft(LeftPadding.LeftPad, LeftPadding.PaddingCharacter)}{level} {message}";
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
        Log("ℹ️", message, ConsoleColor.White);
    }

    /// <summary>
    /// Logs an "ok" message.
    /// </summary>
    public static void Ok(string message)
    {
        Log("✔", message, ConsoleColor.White);
    }

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    public static void Warn(string message)
    {
        Log("❗", message, ConsoleColor.Yellow);
    }

    /// <summary>
    /// Logs an error message.
    /// </summary>
    public static void Error(string message)
    {
        Log("❌", message, ConsoleColor.Red);
    }

    /// <summary>
    /// Logs a debug message.
    /// </summary>
    public static void Debug(string message)
    {
        Log("🔧", message, ConsoleColor.Green); // Are you a 🔧 monerator?
    }

    /// <summary>
    /// Inserts a blank line.
    /// </summary>
    public static void NewLine()
    {
        Log("\r\n", string.Empty, ConsoleColor.Black);
    }
}
