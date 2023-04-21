namespace ETHTPS.Configuration.Validation.Exceptions
{
    public class ConfigurationStringNotFoundException : Exception
    {
        public string ConfigurationStringName { get; private set; }
        public string MicroverviceName { get; set; }

        public ConfigurationStringNotFoundException(string configurationStringName, string microverviceName)
        {
            ConfigurationStringName = configurationStringName;
            MicroverviceName = microverviceName;
        }
    }
}
