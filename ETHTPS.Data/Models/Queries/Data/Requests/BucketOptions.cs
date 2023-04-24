using System;

namespace ETHTPS.Data.Core.Models.Queries.Data.Requests
{
    public sealed class BucketOptions
    {
        public bool UseTimeBuckets { get; set; } = true;
        public TimeInterval BucketSize { get; set; } = TimeInterval.Auto;
        public TimeSpan? CustomBucketSize { get; set; }
    }
}
