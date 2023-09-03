namespace ETHTPS.Data.Integrations.MSSQL.RPC;

public partial class HealthStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Details { get; set; }

    public int? SeverityLevel { get; set; }

    public virtual ICollection<Health> Healths { get; set; } = new List<Health>();
}
