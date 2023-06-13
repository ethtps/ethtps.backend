namespace ETHTPS.Data.Integrations.MSSQL;

public partial class DetailedAccessStat
{
    public int Id { get; set; }

    public required string Path { get; set; }

    public double RequestTimeMs { get; set; }

    public required string Ipaddress { get; set; }

    public DateTime Date { get; set; }
}
