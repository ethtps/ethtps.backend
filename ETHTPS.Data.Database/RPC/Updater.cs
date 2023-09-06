using Newtonsoft.Json;

namespace ETHTPS.Data.Integrations.MSSQL.RPC;

public partial class Updater
{
    public int Id { get; set; }

    public int Network { get; set; }

    public int Provider { get; set; }

    public string? Description { get; set; }

    public DateTime? LastUpdated { get; set; }
    [JsonIgnore]

    public virtual ICollection<Binding> Bindings { get; set; } = new List<Binding>();
    [JsonIgnore]

    public virtual Network NetworkNavigation { get; set; } = null!;
    [JsonIgnore]

    public virtual Provider ProviderNavigation { get; set; } = null!;

    [JsonIgnore]

    public virtual ICollection<UpdaterConfiguration> UpdaterConfigurations { get; set; } = new List<UpdaterConfiguration>();
}
