namespace ETHTPS.Configuration
{
    public interface IConfigurationString
    {
        /// <summary>
        /// Gets 
        /// </summary>
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsSecret { get; set; }
        public bool IsEncrypted { get; set; }
        public string? EncryptionAlgorithmOrHint { get; set; }
    }

    public sealed class ConfigurationStringUpdateModel : IConfigurationString
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public required string Value { get; set; }
        public bool IsSecret { get; set; }
        public bool IsEncrypted { get; set; }
        public string? EncryptionAlgorithmOrHint { get; set; }
    }
}
