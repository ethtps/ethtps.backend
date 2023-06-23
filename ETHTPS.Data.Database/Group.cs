using ETHTPS.Data.Core;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Group : IIndexed
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public virtual ICollection<ApikeyGroup> ApikeyGroups { get; } = new List<ApikeyGroup>();

    public virtual ICollection<GroupRole> GroupRoles { get; } = new List<GroupRole>();
}
