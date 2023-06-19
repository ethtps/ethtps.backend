namespace ETHTPS.Configuration.Database;

public partial class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ConfigurationStringTag> ConfigurationStringTags { get; set; } = new List<ConfigurationStringTag>();

    public virtual ICollection<MicroserviceTag> MicroserviceTags { get; set; } = new List<MicroserviceTag>();

    public virtual ICollection<ProviderTag> ProviderTags { get; set; } = new List<ProviderTag>();
}
