namespace ETHTPS.Configuration.Database;

public partial class Environment
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static Environment EMPTY = new()
#pragma warning restore CA2211 // Non-constant fields should not be visible
    {
        Id = -1,
        Name = string.Empty
    };
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MicroserviceConfigurationString> MicroserviceConfigurationStrings { get; set; } = new List<MicroserviceConfigurationString>();

    public virtual ICollection<ProviderConfigurationString> ProviderConfigurationStrings { get; set; } = new List<ProviderConfigurationString>();
}
