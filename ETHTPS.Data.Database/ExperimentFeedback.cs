﻿using ETHTPS.Data.Core;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class ExperimentFeedback : IIndexed
{
    public int Id { get; set; }

    public int Experiment { get; set; }

    public bool? Vote { get; set; }

    public int? Rating { get; set; }

    public required string Text { get; set; }

    public virtual Experiment? ExperimentNavigation { get; set; }
}
