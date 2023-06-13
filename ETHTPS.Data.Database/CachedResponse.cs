namespace ETHTPS.Data.Integrations.MSSQL;

public partial class CachedResponse
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string KeyJson { get; set; }

    public required string ValueJson { get; set; }
}
