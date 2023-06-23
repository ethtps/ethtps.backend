using ETHTPS.Data.Core;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class ExperimentTarget : IIndexed
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public int Type { get; set; }

    public required virtual ICollection<Experiment> Experiments { get; set; }

    public virtual ExperimentTargetType? TypeNavigation { get; set; }
}
