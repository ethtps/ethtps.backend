namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Server
{
    public required string Id { get; set; }

    public required string Data { get; set; }

    public DateTime LastHeartbeat { get; set; }
}
