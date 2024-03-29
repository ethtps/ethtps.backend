﻿using ETHTPS.Data.Core;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class ExperimentRunParameter : IIndexed
{
    public int Id { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool Enabled { get; set; }

    public int? DisplayToNpeopleBeforeEnd { get; set; }

    public int ConsiderFinishedAfterTimeoutSeconds { get; set; }

    public int? EnrollmentChance { get; set; }

    public virtual ICollection<Experiment> Experiments { get; } = new List<Experiment>();
}
