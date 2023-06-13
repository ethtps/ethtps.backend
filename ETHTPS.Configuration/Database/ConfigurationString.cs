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

    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Value { get; set; }

    public virtual ICollection<MicroserviceConfigurationString> MicroserviceConfigurationStrings { get; } = new List<MicroserviceConfigurationString>();

    public virtual ICollection<ProviderConfigurationString> ProviderConfigurationStrings { get; } = new List<ProviderConfigurationString>();
}
