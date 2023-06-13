namespace ETHTPS.Data.Integrations.MSSQL;

public partial class JobParameter
{
    public long JobId { get; set; }

    public required string Name { get; set; }

    public required string Value { get; set; }

    public virtual Job? Job { get; set; }
}
