namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Set
{
    public required string Key { get; set; }

    public double Score { get; set; }

    public required string Value { get; set; }

    public DateTime? ExpireAt { get; set; }
}
