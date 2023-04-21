namespace ETHTPS.Services
{
    public static class CronConstants
    {
        public const string EVERY_5_S = "*/5 * * * * *";
        public const string EVERY_10_S = "*/10 * * * * *";
        public const string EVERY_13_S = "*/13 * * * * *";
        public const string EVERY_30_S = "*/30 * * * * *";
        public const string EVERY_MINUTE = "* * * * *";
        public const string EVERY_5_MINUTES = "*/5 * * * *";
        public const string EVERY_15_MINUTES = "*/15 * * * *";
        public const string EVERY_HOUR = "0 * * * *";
        public const string NEVER = "0 0 5 31 2 ?";
        public const string EVERY_MIDNIGHT = "0 0 * * *";
    }
}
