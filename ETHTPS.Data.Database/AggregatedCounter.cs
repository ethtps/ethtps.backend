using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class AggregatedCounter
{
    public string Key { get; set; } = string.Empty;

    public long Value { get; set; }

    public DateTime? ExpireAt { get; set; }
}
