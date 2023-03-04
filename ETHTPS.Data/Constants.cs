using System;

namespace ETHTPS.Data.Core
{
    public static class Constants
    {
        public static class CacheTimes
        {
            public static TimeSpan Realtime = TimeSpan.FromSeconds(3);
            public static TimeSpan LowUpdateRate = TimeSpan.FromMinutes(1);
        }

        public static class EnvironmentVariables
        {
            public static string ENV = "ETHTPS_ENV";
            public static string BASE_DIR = "ETHTPS_BASE_DIR";
            public static string CONFIGURATION_PROVIDER_CONN_STR = "ETHTPS_CONFIGURATION_PROVIDER_DB_CONN_STR";
            public static class API
            {
                public static string LOCATION = "ETHTPS_API_DIR";
                public static string NAME = "ETHTPS.API";
            }
            public static class WSAPI
            {
                public static string LOCATION = "ETHTPS_WSAPI_DIR";
                public static string NAME = "ETHTPS.WSAPI";
            }
        }

        public static class Headers
        {
            public static string XAPIKey => "X-API-Key";
        }

        public static class TimeConstants
        {
            public static TimeSpan OneMinute = TimeSpan.FromSeconds(60);
        }

        public static string All => "All";
        public static string Mainnet => "Mainnet";
    }
}
