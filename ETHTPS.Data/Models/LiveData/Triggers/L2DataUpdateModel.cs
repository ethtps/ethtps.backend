using System.Collections.Generic;

namespace ETHTPS.Data.Core.Models.LiveData.Triggers
{
    public sealed class L2DataUpdateModel
    {
        public string Provider { get; set; }
        public MinimalDataPoint? Data { get; set; }
        public int? BlockNumber { get; set; }
        public IEnumerable<TransactionMetadata>? Transactions { get; set; }
    }
}
