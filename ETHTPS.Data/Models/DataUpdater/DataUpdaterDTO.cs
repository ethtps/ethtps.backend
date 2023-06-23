using System;

namespace ETHTPS.Data.Core.Models.DataUpdater
{
    public sealed class DataUpdaterDTO
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Provider { get; set; }
        public bool Enabled { get; set; }
        public string Status { get; set; }
        public DateTime? LastSuccessfulRunTime { get; set; }
        public DateTime? LastRunTime { get; set; }
        public int NumberOfSuccesses { get; set; }
        public int NumberOfFailures { get; set; }
    }

}
