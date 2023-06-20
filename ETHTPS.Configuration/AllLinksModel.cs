using System.ComponentModel.DataAnnotations.Schema;

namespace ETHTPS.Configuration
{
    /// <summary>
    /// Represents a row returned by the [Configuration].[GetAllLinksForConfigurationString] procedure
    /// </summary>
    public class AllLinksModel
    {
        [Column("ConfigurationString.ID")]
        public required int ConfigurationStringID { get; set; }

        [Column("ConfigurationString.Name")]
        public required string ConfigurationStringName { get; set; }

        [Column("ConfigurationString.Value")]
        public required string ConfigurationStringValue { get; set; }

        [Column("ProviderLinks.ID")]
        public int? ProviderLinksID { get; set; }

        [Column("ProviderLinks.ProviderID")]
        public int? ProviderLinksProviderID { get; set; }

        [Column("ProviderLinks.ConfigurationStringID")]
        public int? ProviderLinksConfigurationStringID { get; set; }

        [Column("ProviderLinks.EnvironmentID")]
        public int? ProviderLinksEnvironmentID { get; set; }

        [Column("MicroserviceLinks.ID")]
        public int? MicroserviceLinksID { get; set; }

        [Column("MicroserviceLinks.MicroserviceID")]
        public int? MicroserviceLinksMicroserviceID { get; set; }

        [Column("MicroserviceLinks.ConfigurationStringID")]
        public int? MicroserviceLinksConfigurationStringID { get; set; }

        [Column("MicroserviceLinks.EnvironmentID")]
        public int? MicroserviceLinksEnvironmentID { get; set; }
    }

}
