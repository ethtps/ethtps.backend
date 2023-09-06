using Newtonsoft.Json;

namespace ETHTPS.Data.Integrations.MSSQL.RPC;

public partial class UpdaterConfiguration
{
    public int Id { get; set; }

    public int Updater { get; set; }

    public bool? Enabled { get; set; }

    public int UpdateIntervalMs { get; set; }
    public string? AuthMethod { get; set; }
    public string? AuthMethodDetails { get; set; }

    public int? MaxRetries { get; set; }

    public int? RetryIntervalMs { get; set; }
    [JsonIgnore]

    public virtual Updater UpdaterNavigation { get; set; } = null!;
}
