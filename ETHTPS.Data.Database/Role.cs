namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Role
{
    public int Id { get; set; }

    public required string Name { get; set; } = string.Empty;

    public virtual ICollection<GroupRole> GroupRoles { get; } = new List<GroupRole>();

    public virtual ICollection<PermissionsForRole> PermissionsForRoles { get; } = new List<PermissionsForRole>();
}
