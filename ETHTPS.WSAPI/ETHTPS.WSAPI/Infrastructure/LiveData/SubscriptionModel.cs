using ETHTPS.Data.Core;

namespace ETHTPS.WSAPI.Infrastructure.LiveData
{
    public sealed class SubscriptionModel
    {
        private DateTime ConnectionTime { get; set; }
        public TimeSpan TimeAlive => DateTime.Now - ConnectionTime;

        public SubscriptionModel()
        {
            ConnectionTime = DateTime.Now;
        }

        public DataType[]? DataTypes { get; set; }
        public bool? IncludeSidechains { get; set; }
        public bool? IncludeTransactions { get; set; }
    }
}
