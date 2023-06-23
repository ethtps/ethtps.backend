using System.Collections.Generic;

namespace ETHTPS.Data.Core.Models.LiveData.Triggers
{
    public sealed class L2DataUpdateModel : ICachedKey
    {
        public string Provider { get; set; }
        public MinimalDataPoint? Data { get; set; }
        public int? BlockNumber { get; set; }
        public IEnumerable<TransactionMetadata>? Transactions { get; set; }

        /// <summary>
        /// Gets or sets an optional cache key.If not set, the default cache key will be used.
        /// </summary>
        public string? CacheKey { get; set; }
        public string ToCacheKey() => CacheKey ?? $"{Provider}:L2DataUpdateModel:{BlockNumber}";
    }
}
