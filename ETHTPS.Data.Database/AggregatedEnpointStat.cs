﻿namespace ETHTPS.Data.Integrations.MSSQL
{
    public partial class AggregatedEnpointStat
    {
        public int Id { get; set; }
        public string Path { get; set; } = string.Empty;
        public int AverageRequestTimeMs { get; set; }
        public int RequestCount { get; set; }
    }
}
