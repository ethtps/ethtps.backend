using ETHTPS.Data.Core;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Experiment : IIndexed
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "We need this")]
    public static Experiment EMPTY = new()
    {
        Id = -1,
        ProjectId = -1,
        Name = "EMPTY",
        Description = "EMPTY",
        Target = -1,
    };
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public int Target { get; set; }

    public int RunParameters { get; set; }

    public virtual ICollection<ApikeyExperimentBinding> ApikeyExperimentBindings { get; } = new List<ApikeyExperimentBinding>();

    public virtual ICollection<ExperimentFeedback> ExperimentFeedbacks { get; } = new List<ExperimentFeedback>();

    public virtual ICollection<ExperimentResult> ExperimentResults { get; } = new List<ExperimentResult>();

    public virtual ExperimentalSession? ExperimentalSession { get; set; }

    public virtual Provider? Project { get; set; }

    public virtual ExperimentRunParameter? RunParametersNavigation { get; set; }

    public virtual ExperimentTarget? TargetNavigation { get; set; }
}
