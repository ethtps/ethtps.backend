﻿namespace ETHTPS.Configuration.Validation.Exceptions
{
    public class InvalidConfigurationStringException : Exception
    {
        public string ConfigurationStringName { get; private set; }
        public string? Reason { get; private set; }

        public InvalidConfigurationStringException(string configurationString)
        {
            ConfigurationStringName = configurationString;
        }

        public InvalidConfigurationStringException(string configurationStringName, string reason)
        {
            ConfigurationStringName = configurationStringName;
            Reason = reason;
        }
    }
}
