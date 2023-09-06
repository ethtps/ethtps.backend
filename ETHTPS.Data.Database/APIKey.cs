using ETHTPS.Data.Core;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Apikey : IIndexed
{
    public int Id { get; set; }

    public string KeyHash { get; set; } = string.Empty;

    public int TotalCalls { get; set; }

    public int CallsLast24h { get; set; }

    public int Limit24h { get; set; }

    public string RequesterIpaddress { get; set; } = string.Empty;

    public virtual ICollection<ApikeyExperimentBinding> ApikeyExperimentBindings { get; } = new List<ApikeyExperimentBinding>();

    public virtual ICollection<ApikeyGroup> ApikeyGroups { get; } = new List<ApikeyGroup>();
}
