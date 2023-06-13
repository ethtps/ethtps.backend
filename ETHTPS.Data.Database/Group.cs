﻿namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Group
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public virtual ICollection<ApikeyGroup> ApikeyGroups { get; } = new List<ApikeyGroup>();

    public virtual ICollection<GroupRole> GroupRoles { get; } = new List<GroupRole>();
}
