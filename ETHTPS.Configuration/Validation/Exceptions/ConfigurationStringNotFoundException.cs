namespace ETHTPS.Configuration.Validation.Exceptions
{
    public sealed class ConfigurationStringNotFoundException : Exception
    {
        public string ConfigurationStringName { get; private set; }
        public string Requester { get; set; }

        public ConfigurationStringNotFoundException(string configurationStringName, string requester)
        {
            ConfigurationStringName = configurationStringName;
            Requester = requester;
        }

        public override string Message => $"Configuration string {ConfigurationStringName} for {Requester} not found";
    }
}
