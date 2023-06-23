namespace ETHTPS.Data.Integrations.MSSQL;

public partial class State
{
    public long Id { get; set; }

    public long JobId { get; set; }

    public required string Name { get; set; }

    public required string Reason { get; set; }

    public DateTime CreatedAt { get; set; }

    public required string Data { get; set; }

    public virtual Job? Job { get; set; }
}
