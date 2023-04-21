using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL.Temp
{
    public partial class Permission
    {
        public Permission()
        {
            PermissionsForRoles = new HashSet<PermissionsForRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<PermissionsForRole> PermissionsForRoles { get; set; }
    }
}
