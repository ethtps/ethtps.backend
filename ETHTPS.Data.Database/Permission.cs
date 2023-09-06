using ETHTPS.Data.Core;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Permission : IIndexed
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<PermissionsForRole> PermissionsForRoles { get; } = new List<PermissionsForRole>();
}
