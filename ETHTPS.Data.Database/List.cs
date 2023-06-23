namespace ETHTPS.Data.Integrations.MSSQL;

public partial class List
{
    public long Id { get; set; }

    public required string Key { get; set; }

    public required string Value { get; set; }

    public DateTime? ExpireAt { get; set; }
}
