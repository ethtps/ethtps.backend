using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL.Temp
{
    public partial class ExperimentTarget
    {
        public ExperimentTarget()
        {
            Experiments = new HashSet<Experiment>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Type { get; set; }

        public virtual ExperimentTargetType TypeNavigation { get; set; } = null!;
        public virtual ICollection<Experiment> Experiments { get; set; }
    }
}
