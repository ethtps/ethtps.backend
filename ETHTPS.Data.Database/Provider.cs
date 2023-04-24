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
        ProviderDetailsMarkdownPages = new HashSet<ProviderDetailsMarkdownPage>();
        ProviderLinks = new HashSet<ProviderLink>();
        TimeWarpData = new HashSet<TimeWarpDatum>();
        TimeWarpDataDays = new HashSet<TimeWarpDataDay>();
        TimeWarpDataHours = new HashSet<TimeWarpDataHour>();
        TimeWarpDataMinutes = new HashSet<TimeWarpDataMinute>();
        TimeWarpDataWeeks = new HashSet<TimeWarpDataWeek>();
        TpsandGasDataAlls = new HashSet<TpsandGasDataAll>();
        TpsandGasDataDays = new HashSet<TpsandGasDataDay>();
        TpsandGasDataHours = new HashSet<TpsandGasDataHour>();
        TpsandGasDataMinutes = new HashSet<TpsandGasDataMinute>();
        TpsandGasDataMonths = new HashSet<TpsandGasDataMonth>();
        TpsandGasDataWeeks = new HashSet<TpsandGasDataWeek>();
        TpsandGasDataYears = new HashSet<TpsandGasDataYear>();
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
    public virtual TpsandGasDataLatest? TpsandGasDataLatest { get; set; }
    public virtual TpsandGasDataMax? TpsandGasDataMax { get; set; }
    public virtual ICollection<DataUpdater> DataUpdaters { get; set; }
    public virtual ICollection<Experiment> Experiments { get; set; }
    public virtual ICollection<Provider> InverseSubchainOfNavigation { get; set; }
    public virtual ICollection<OldestLoggedHistoricalEntry> OldestLoggedHistoricalEntries { get; set; }
    public virtual ICollection<OldestLoggedTimeWarpBlock> OldestLoggedTimeWarpBlocks { get; set; }
    public virtual ICollection<Project> Projects { get; set; }
    public virtual ICollection<ProviderDetailsMarkdownPage> ProviderDetailsMarkdownPages { get; set; }
    public virtual ICollection<ProviderLink> ProviderLinks { get; set; }
    public virtual ICollection<TimeWarpDatum> TimeWarpData { get; set; }
    public virtual ICollection<TimeWarpDataDay> TimeWarpDataDays { get; set; }
    public virtual ICollection<TimeWarpDataHour> TimeWarpDataHours { get; set; }
    public virtual ICollection<TimeWarpDataMinute> TimeWarpDataMinutes { get; set; }
    public virtual ICollection<TimeWarpDataWeek> TimeWarpDataWeeks { get; set; }
    public virtual ICollection<TpsandGasDataAll> TpsandGasDataAlls { get; set; }
    public virtual ICollection<TpsandGasDataDay> TpsandGasDataDays { get; set; }
    public virtual ICollection<TpsandGasDataHour> TpsandGasDataHours { get; set; }
    public virtual ICollection<TpsandGasDataMinute> TpsandGasDataMinutes { get; set; }
    public virtual ICollection<TpsandGasDataMonth> TpsandGasDataMonths { get; set; }
    public virtual ICollection<TpsandGasDataWeek> TpsandGasDataWeeks { get; set; }
    public virtual ICollection<TpsandGasDataYear> TpsandGasDataYears { get; set; }
}
