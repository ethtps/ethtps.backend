using ETHTPS.Data.Core;

namespace ETHTPS.WSAPI.Infrastructure.LiveData
{
    public class SubscriptionModel
    {
        public DataType[]? DataTypes { get; set; }
        public bool? IncludeSidechains { get; set; }
        public bool? IncludeTransactions { get; set; }
    }
}
