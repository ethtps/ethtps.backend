namespace ETHTPS.Configuration.Validation.Exceptions
{
    public sealed class ConfigurationStringNotFoundException : Exception
    {
        public string ConfigurationStringName { get; private set; }
        public string MicroserviceName { get; set; }

        public ConfigurationStringNotFoundException(string configurationStringName, string microserviceName)
        {
            ConfigurationStringName = configurationStringName;
            MicroserviceName = microserviceName;
        }

        public override string Message => $"Configuration string {ConfigurationStringName} for {MicroserviceName} not found";
    }
}
