namespace ETHTPS.Data.Integrations.MSSQL;

public partial class AppConfigurationValue
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Value { get; set; }
}
