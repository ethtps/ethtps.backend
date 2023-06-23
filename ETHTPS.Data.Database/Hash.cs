namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Hash
{
    public required string Key { get; set; }

    public required string Field { get; set; }

    public required string Value { get; set; }

    public DateTime? ExpireAt { get; set; }
}
