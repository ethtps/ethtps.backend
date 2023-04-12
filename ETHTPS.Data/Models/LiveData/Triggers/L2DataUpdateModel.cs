namespace ETHTPS.Data.Core.Models.LiveData.Triggers
{
    public class L2DataUpdateModel
    {
        public string Provider { get; set; }
        public MinimalDataPoint Data { get; set; }
        public TransactionMetadata? Metadata { get; set; }
    }
}
