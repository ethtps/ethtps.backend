using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL.Temp
{
    public partial class ExperimentTargetType
    {
        public ExperimentTargetType()
        {
            ExperimentTargets = new HashSet<ExperimentTarget>();
        }

        public int Id { get; set; }
        public string TargetTypeName { get; set; } = null!;
        public string TargetTypeValue { get; set; } = null!;

        public virtual ICollection<ExperimentTarget> ExperimentTargets { get; set; }
    }
}
