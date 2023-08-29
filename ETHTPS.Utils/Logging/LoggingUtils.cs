namespace ETHTPS.Utils.Logging
{
    /// <summary>
    /// Some quick logging utils for debugging purposes.
    /// </summary>
    public static class LoggingUtils
    {
        public static void Trace(string message)
        {
#if DEBUG
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("trace: ");
            Console.WriteLine($"{message}");
            Console.ResetColor();
#endif
        }
    }
}
