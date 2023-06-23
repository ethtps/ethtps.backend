namespace ETHTPS.Configuration.Validation.Models
{
    public sealed class StartupConfigurationModel
    {
        public RequiredConfigurationModel? Required { get; set; }
        public OptionalConfigurationModel? Optional { get; set; }
    }
}