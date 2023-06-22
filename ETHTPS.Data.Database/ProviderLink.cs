using ETHTPS.Data.Core;

using Newtonsoft.Json;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class ProviderLink : IIndexed
{
    public int Id { get; set; }

    public int ProviderId { get; set; }

    public int ExternalWebsiteId { get; set; }

    public string? Link { get; set; }
    [JsonIgnore]
    public virtual ExternalWebsite? ExternalWebsite { get; set; }
    [JsonIgnore]
    public virtual Provider? Provider { get; set; }
}
