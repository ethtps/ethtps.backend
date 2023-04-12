using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL.Temp
{
    public partial class Role
    {
        public Role()
        {
            GroupRoles = new HashSet<GroupRole>();
            PermissionsForRoles = new HashSet<PermissionsForRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<GroupRole> GroupRoles { get; set; }
        public virtual ICollection<PermissionsForRole> PermissionsForRoles { get; set; }
    }
}
