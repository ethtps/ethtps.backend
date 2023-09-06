namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ProviderTag> ProviderTags { get; set; } = new List<ProviderTag>();
}
