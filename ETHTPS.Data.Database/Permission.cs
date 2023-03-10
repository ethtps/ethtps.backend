using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Permission
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<PermissionsForRole> PermissionsForRoles { get; } = new List<PermissionsForRole>();
}
