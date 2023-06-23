using ETHTPS.Data.Core;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class GroupRole : IIndexed
{
    public int Id { get; set; }

    public int GroupId { get; set; }

    public int RoleId { get; set; }

    public virtual Group? Group { get; set; }

    public virtual Role? Role { get; set; }
}
