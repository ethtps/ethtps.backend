namespace ETHTPS.Data.Core.Models.LiveData
{
    public sealed class MinimalDataPoint
    {
        public double? TPS { get; set; }
        public double? GPS { get; set; }
        public TransactionMetadata[]? Transactions { get; set; }
    }
}
