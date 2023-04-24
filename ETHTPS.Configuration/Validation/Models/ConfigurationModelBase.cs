namespace ETHTPS.Configuration.Validation.Models
{
    public abstract class ConfigurationModelBase
    {
        public ConfigurationFieldsDescriptor[]? MicroserviceConfiguration { get; set; }
        public ConfigurationFieldsDescriptor[]? ProviderConfiguration { get; set; }
    }
}
