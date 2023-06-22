using ETHTPS.Data.Core;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class ExternalWebsiteCategory : IIndexed
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public virtual ICollection<ExternalWebsite> ExternalWebsites { get; } = new List<ExternalWebsite>();
}
