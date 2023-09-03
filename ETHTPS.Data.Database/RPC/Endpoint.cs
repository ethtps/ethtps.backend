namespace ETHTPS.Data.Integrations.MSSQL.RPC;

public partial class Endpoint
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public bool? Enabled { get; set; }

    public string? Description { get; set; }

    public string? AuthType { get; set; }

    public virtual ICollection<Binding> Bindings { get; set; } = new List<Binding>();
}
