using InfluxDB.Client.Core;

namespace ETHTPS.Data.Core.Models.DataEntries
{
    [Measurement("tps")]
    public sealed class TPSInfo : InfoBase
    {
        [Column("tps")]
        public double TPS { get; set; }
    }
}
