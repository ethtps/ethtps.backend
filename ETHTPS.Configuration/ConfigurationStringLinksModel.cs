using ETHTPS.Configuration.Database;

namespace ETHTPS.Configuration
{
    /// <summary>
    /// Represents a model that contains a <see cref="ConfigurationString"/> and its links to <see cref="ProviderConfigurationString"/> and <see cref="MicroserviceConfigurationString"/>, if they exist.
    /// </summary>
    public class ConfigurationStringLinksModel
    {
        /// <summary>
        /// Gets or sets the <see cref="ConfigurationString"/> object.
        /// </summary>
        public required ConfigurationString ConfigurationString { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ProviderConfigurationString"/> objects that link to the <see cref="ConfigurationString"/>, if they exist.
        /// </summary>
        public IList<ProviderConfigurationString?>? ProviderLinks { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MicroserviceConfigurationString"/> objects that link to the <see cref="ConfigurationString"/>, if they exist.
        /// </summary>
        public IList<MicroserviceConfigurationString?>? MicroserviceLinks { get; set; }
    }
}
