using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL.Temp
{
    public partial class AggregatedEnpointStat
    {
        public int Id { get; set; }
        public string Path { get; set; } = null!;
        public int AverageRequestTimeMs { get; set; }
        public int RequestCount { get; set; }
    }
}
