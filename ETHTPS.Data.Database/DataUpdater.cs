namespace ETHTPS.Data.Integrations.MSSQL;

public partial class DataUpdater
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static DataUpdater EMPTY = new();
#pragma warning restore CA2211 // Non-constant fields should not be visible
    public int Id { get; set; }

    public int TypeId { get; set; }

    public int ProviderId { get; set; }
    public bool? Enabled { get; set; }

    public virtual LiveDataUpdaterStatus? LiveDataUpdaterStatus { get; set; }

    public virtual Provider? Provider { get; set; } = null!;

    public virtual DataUpdaterType? Type { get; set; } = null!;
}
