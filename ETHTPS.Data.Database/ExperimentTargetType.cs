namespace ETHTPS.Data.Integrations.MSSQL;

public partial class ExperimentTargetType
{
    public int Id { get; set; }

    public required string TargetTypeName { get; set; }

    public required string TargetTypeValue { get; set; }

    public virtual ICollection<ExperimentTarget> ExperimentTargets { get; } = new List<ExperimentTarget>();
}
