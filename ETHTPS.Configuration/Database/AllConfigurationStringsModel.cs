using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;

namespace ETHTPS.Configuration.Database
{
    public partial class AllConfigurationStringsModel : IConfigurationString
    {
        public required int ID { get; set; }
        public required string Name { get; set; }
        public required string Value { get; set; }

        private string? _microserviceEnvironments;

        /// <summary>
        /// Gets or sets the JSON value representing the microservice-environment pairs in which this configuration string is used.
        /// </summary>
        [JsonIgnore]
        public string? MicroserviceEnvironments
        {
            get => _microserviceEnvironments;
            set
            {
                if (_microserviceEnvironments == null) return;

                MicroserviceEnvironmentPairs = JsonConvert.DeserializeObject<MicroserviceEnvironment[]>(_microserviceEnvironments);
                _microserviceEnvironments = value;
            }
        }

        /// <summary>
        /// Gets or sets the microservice-environment pairs.
        /// </summary>
        [NotMapped]
        public IEnumerable<MicroserviceEnvironment>? MicroserviceEnvironmentPairs { get; private set; }

        private string? _providerEnvironments;

        /// <summary>
        /// Gets or sets the JSON value representing the provider-environment pairs in which this configuration string is used.
        /// </summary>
        [JsonIgnore]
        public string? ProviderEnvironments
        {
            get => _providerEnvironments;
            set
            {
                if (_providerEnvironments == null) return;

                ProviderEnvironmentPairs = JsonConvert.DeserializeObject<ProviderEnvironment[]>(_providerEnvironments);
                _providerEnvironments = value;
            }
        }
        /// <summary>
        /// Gets or sets the provider-environment pairs.
        /// </summary>
        [NotMapped]
        public IEnumerable<ProviderEnvironment>? ProviderEnvironmentPairs { get; private set; }
        public bool IsSecret { get; set; }
        public bool IsEncrypted { get; set; }
        public string? EncryptionAlgorithmOrHint { get; set; }
    }
}
