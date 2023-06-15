using ETHTPS.Data.Core.Models.Providers;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Provider : ProviderSummaryBase
{
    public Provider()
    {
        DataUpdaters = new HashSet<DataUpdater>();
        Experiments = new HashSet<Experiment>();
        InverseSubchainOfNavigation = new HashSet<Provider>();
        OldestLoggedHistoricalEntries = new HashSet<OldestLoggedHistoricalEntry>();
        OldestLoggedTimeWarpBlocks = new HashSet<OldestLoggedTimeWarpBlock>();
        Projects = new HashSet<Project>();
        ProviderLinks = new HashSet<ProviderLink>();
    }

    public int Type { get; set; }
    public string Color { get; set; } = null!;
    public bool? IsGeneralPurpose { get; set; }
    public int? HistoricalAggregationDeltaBlock { get; set; }
    public bool Enabled { get; set; }
    public int? SubchainOf { get; set; }
    public int TheoreticalMaxTps { get; set; }

    public virtual Provider? SubchainOfNavigation { get; set; }
    public virtual ProviderType TypeNavigation { get; set; } = null!;
    public virtual ICollection<DataUpdater> DataUpdaters { get; set; }
    public virtual ICollection<Experiment> Experiments { get; set; }
    public virtual ICollection<Provider> InverseSubchainOfNavigation { get; set; }
    public virtual ICollection<OldestLoggedHistoricalEntry> OldestLoggedHistoricalEntries { get; set; }
    public virtual ICollection<OldestLoggedTimeWarpBlock> OldestLoggedTimeWarpBlocks { get; set; }
    public virtual ICollection<Project> Projects { get; set; }
    public virtual ICollection<ProviderLink> ProviderLinks { get; set; }

#pragma warning disable IDE0090 // Use 'new(...)'
    public static Provider EMPTY = new Provider();
#pragma warning restore IDE0090 // Use 'new(...)'
}
