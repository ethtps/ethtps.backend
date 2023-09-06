using Newtonsoft.Json;

namespace ETHTPS.Data.Integrations.MSSQL.RPC;

public partial class Endpoint
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public bool? Enabled { get; set; }

    public string? Description { get; set; }

    public string? AuthType { get; set; }
    [JsonIgnore]

    public virtual ICollection<Binding> Bindings { get; set; } = new List<Binding>();
}
