namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Project
{
    public int Id { get; set; }

    public bool Enabled { get; set; }

    public int? Provider { get; set; }

    public required string Name { get; set; }

    public required string Website { get; set; }

    public required string Details { get; set; }

    public virtual ICollection<Feature> Features { get; } = new List<Feature>();

    public virtual Provider? ProviderNavigation { get; set; } = new();
}
