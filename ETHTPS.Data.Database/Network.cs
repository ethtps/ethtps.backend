using ETHTPS.Data.Core;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Network : IIndexed
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static Network EMPTY = new() { Name = "invalid" };
#pragma warning restore CA2211 // Non-constant fields should not be visible
    public int Id { get; set; }

    public required string Name { get; set; }

    public virtual ICollection<OldestLoggedHistoricalEntry> OldestLoggedHistoricalEntries { get; } = new List<OldestLoggedHistoricalEntry>();

    public virtual ICollection<OldestLoggedTimeWarpBlock>? OldestLoggedTimeWarpBlocks { get; }

    public virtual ICollection<StarkwareTransactionCountDatum>? StarkwareTransactionCountData { get; }
}
