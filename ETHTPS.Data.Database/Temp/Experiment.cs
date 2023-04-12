using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL.Temp
{
    public partial class Experiment
    {
        public Experiment()
        {
            ApikeyExperimentBindings = new HashSet<ApikeyExperimentBinding>();
            ExperimentFeedbacks = new HashSet<ExperimentFeedback>();
            ExperimentResults = new HashSet<ExperimentResult>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Target { get; set; }
        public int RunParameters { get; set; }

        public virtual Provider Project { get; set; } = null!;
        public virtual ExperimentRunParameter RunParametersNavigation { get; set; } = null!;
        public virtual ExperimentTarget TargetNavigation { get; set; } = null!;
        public virtual ICollection<ApikeyExperimentBinding> ApikeyExperimentBindings { get; set; }
        public virtual ICollection<ExperimentFeedback> ExperimentFeedbacks { get; set; }
        public virtual ICollection<ExperimentResult> ExperimentResults { get; set; }
    }
}
