namespace ETHTPS.Configuration.Validation.Models
{
    public class StartupConfigurationModel
    {
        public RequiredConfigurationModel? Required { get; set; }
        public OptionalConfigurationModel? Optional { get; set; }
    }
}