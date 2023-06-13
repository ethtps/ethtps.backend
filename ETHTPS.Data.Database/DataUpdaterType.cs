namespace ETHTPS.Data.Integrations.MSSQL;

public partial class DataUpdaterType
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static DataUpdaterType EMPTY = new() { TypeName = "none" };
#pragma warning restore CA2211 // Non-constant fields should not be visible
    public int Id { get; set; }

    public required string TypeName { get; set; }

    public virtual ICollection<DataUpdater> DataUpdaters { get; } = new List<DataUpdater>();
}
