namespace ETHTPS.Data.Integrations.MSSQL;

public partial class StarkwareTransactionCountDatum
{
    public int Id { get; set; }

    public int Network { get; set; }

    public required string Product { get; set; }

    public DateTime LastUpdateTime { get; set; }

    public int LastUpdateCount { get; set; }

    public double LastUpdateTps { get; set; }

    public virtual Network? NetworkNavigation { get; set; }
}
