using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL.Temp
{
    public partial class Group
    {
        public Group()
        {
            ApikeyGroups = new HashSet<ApikeyGroup>();
            GroupRoles = new HashSet<GroupRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<ApikeyGroup> ApikeyGroups { get; set; }
        public virtual ICollection<GroupRole> GroupRoles { get; set; }
    }
}
