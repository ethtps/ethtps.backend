

using Newtonsoft.Json;

namespace ETHTPS.Data.Integrations.MSSQL.RPC;

public partial class Binding
{
    public int Id { get; set; }

    public int Endpoint { get; set; }

    public int Updater { get; set; }

    public bool? IsActive { get; set; }

    public string? LastError { get; set; }
    [JsonIgnore]

    public virtual Endpoint EndpointNavigation { get; set; } = null!;
    [JsonIgnore]

    public virtual ICollection<Health> Healths { get; set; } = new List<Health>();
    [JsonIgnore]

    public virtual Updater UpdaterNavigation { get; set; } = null!;
}
