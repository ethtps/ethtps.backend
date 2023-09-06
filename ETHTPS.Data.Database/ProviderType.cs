using ETHTPS.Data.Core;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class ProviderType : IIndexed
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Color { get; set; }

    public bool IsGeneralPurpose { get; set; }

    public bool Enabled { get; set; }

    public virtual ICollection<Provider> Providers { get; } = new List<Provider>();
}
