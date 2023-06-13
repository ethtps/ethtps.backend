namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Network
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static Network EMPTY = new() { Name = "invalid" };
#pragma warning restore CA2211 // Non-constant fields should not be visible
    public int Id { get; set; }

    public required string Name { get; set; }

    public virtual ICollection<OldestLoggedHistoricalEntry> OldestLoggedHistoricalEntries { get; } = new List<OldestLoggedHistoricalEntry>();

    public virtual ICollection<OldestLoggedTimeWarpBlock>? OldestLoggedTimeWarpBlocks { get; }

    public virtual ICollection<StarkwareTransactionCountDatum>? StarkwareTransactionCountData { get; }

    public virtual ICollection<TimeWarpDatum> TimeWarpData { get; } = new List<TimeWarpDatum>();

    public virtual ICollection<TimeWarpDataDay> TimeWarpDataDays { get; } = new List<TimeWarpDataDay>();

    public virtual ICollection<TimeWarpDataHour> TimeWarpDataHours { get; } = new List<TimeWarpDataHour>();

    public virtual ICollection<TimeWarpDataMinute> TimeWarpDataMinutes { get; } = new List<TimeWarpDataMinute>();

    public virtual ICollection<TimeWarpDataWeek> TimeWarpDataWeeks { get; } = new List<TimeWarpDataWeek>();

    public virtual ICollection<TpsandGasDataAll> TpsandGasDataAlls { get; } = new List<TpsandGasDataAll>();

    public virtual ICollection<TpsandGasDataDay> TpsandGasDataDays { get; } = new List<TpsandGasDataDay>();

    public virtual ICollection<TpsandGasDataHour> TpsandGasDataHours { get; } = new List<TpsandGasDataHour>();

    public virtual ICollection<TpsandGasDataLatest> TpsandGasDataLatests { get; } = new List<TpsandGasDataLatest>();

    public virtual ICollection<TpsandGasDataMax> TpsandGasDataMaxes { get; } = new List<TpsandGasDataMax>();

    public virtual ICollection<TpsandGasDataMinute> TpsandGasDataMinutes { get; } = new List<TpsandGasDataMinute>();

    public virtual ICollection<TpsandGasDataMonth> TpsandGasDataMonths { get; } = new List<TpsandGasDataMonth>();

    public virtual ICollection<TpsandGasDataWeek> TpsandGasDataWeeks { get; } = new List<TpsandGasDataWeek>();

    public virtual ICollection<TpsandGasDataYear> TpsandGasDataYears { get; } = new List<TpsandGasDataYear>();
}
