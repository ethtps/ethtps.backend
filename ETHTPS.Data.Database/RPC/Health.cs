using Newtonsoft.Json;

namespace ETHTPS.Data.Integrations.MSSQL.RPC;

public partial class Health
{
    public int Id { get; set; }

    public int Binding { get; set; }

    public int Status { get; set; }

    public DateTime LastCheck { get; set; }

    public int? ResponseTimeMs { get; set; }

    public string? ErrorDetails { get; set; }
    [JsonIgnore]

    public virtual Binding BindingNavigation { get; set; } = null!;
    [JsonIgnore]

    public virtual HealthStatus StatusNavigation { get; set; } = null!;
}
