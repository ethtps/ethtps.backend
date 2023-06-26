namespace ETHTPS.Configuration.Database;

public partial class ConfigurationString : IConfigurationString
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static ConfigurationString EMPTY = new()
#pragma warning restore CA2211 // Non-constant fields should not be visible
    {
        Id = -1,
        Name = string.Empty,
        Value = string.Empty
    };

    public static ConfigurationString From(IConfigurationString x) => new()
    {
        Name = x.Name,
        Value = x.Value,
        IsSecret = x.IsSecret,
        IsEncrypted = x.IsEncrypted,
        EncryptionAlgorithmOrHint = x.EncryptionAlgorithmOrHint,
    };

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Value { get; set; } = null!;

    public bool IsSecret { get; set; }

    public bool IsEncrypted { get; set; }

    public string? EncryptionAlgorithmOrHint { get; set; }

    public virtual ICollection<ConfigurationStringTag> ConfigurationStringTags { get; set; } = new List<ConfigurationStringTag>();

    public virtual ICollection<MicroserviceConfigurationString> MicroserviceConfigurationStrings { get; set; } = new List<MicroserviceConfigurationString>();

    public virtual ICollection<ProviderConfigurationString> ProviderConfigurationStrings { get; set; } = new List<ProviderConfigurationString>();
}
