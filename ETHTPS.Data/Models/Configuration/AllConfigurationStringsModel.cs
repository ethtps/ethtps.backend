namespace ETHTPS.Data.Core.Models.Configuration
{
    public partial class AllConfigurationStringsModel
    {
        public string ConfigurationString { get; set; }
        public string ConfigurationValue { get; set; }
        public int? MicroserviceID { get; set; }
        public int? ProviderID { get; set; }
        public string Environment { get; set; }
    }
}
