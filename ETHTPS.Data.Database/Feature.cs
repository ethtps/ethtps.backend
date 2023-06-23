namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Feature
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public bool Enabled { get; set; }

    public required string Name { get; set; }

    public required string Details { get; set; }

    public virtual Project? Project { get; set; }
}
