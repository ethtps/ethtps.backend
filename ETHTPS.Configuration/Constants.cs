namespace ETHTPS.Configuration
{
    public static class Constants
    {
        public const string ENVIRONMENT =
#if DEVELOPMENT
            "Debug";
#elif STAGING
            "Staging";
#else
"Release";
#endif
        public const string STARTUP_CONFIG_FILENAME = "StartupConfig.json";
    }
}
