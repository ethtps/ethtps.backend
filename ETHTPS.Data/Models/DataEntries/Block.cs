using System;

using InfluxDB.Client.Core;

namespace ETHTPS.Data.Core.Models.DataEntries
{
    public sealed class Block : IBlock
    {
        public int BlockNumber { get; set; }
        public int TransactionCount { get; set; }
        public int GasUsed { get; set; }
        public DateTime Date { get; set; }
        public bool Settled { get; set; } = true;
        public string Provider { get; set; }
        public string[]? TXHashes { get; set; }
        public override string ToString() => $"{Provider} #{BlockNumber}";
        public static TPSGPSInfo operator -(Block a, Block b) => new()
        {
            Date = a.Date,
            BlockNumber = a.BlockNumber,
            TPS = (a.TransactionCount) / (a.Date.Subtract(b.Date).TotalSeconds),
            GPS = (a.GasUsed) / (a.Date.Subtract(b.Date).TotalSeconds),
            Provider = a.Provider,
        };
    }

    /// <summary>
    /// A double-based implementation of <see cref="IBlock"/> since Influx uses doubles instead of ints
    /// </summary>
    /// <seealso cref="ETHTPS.Data.Core.IMeasurement" />
    [Measurement("blockinfo")]
    public sealed class InfluxBlock : IMeasurement
    {
        [Column("blocknumber", IsTag = false)]
        public double BlockNumber { get; set; }
        [Column("transactioncount", IsMeasurement = true)]
        public double TransactionCount { get; set; }
        [Column("gasused", IsMeasurement = true)]
        public double GasUsed { get; set; }
        [Column("date", IsTimestamp = true)]
        public DateTime Date { get; set; }
        [Column("settled")]
        public bool Settled { get; set; } = true;
        [Column("provider", IsTag = true)]
        public string Provider { get; set; }
        public string[]? TXHashes { get; set; }
        public override string ToString() => $"{Provider} #{BlockNumber}";
        public Block ToBlock() => new()
        {

            BlockNumber = (int)BlockNumber,
            TransactionCount = (int)TransactionCount,
            GasUsed = (int)GasUsed,
            Provider = (string)Provider,
            TXHashes = TXHashes,

        };
        public static TPSGPSInfo operator -(InfluxBlock a, InfluxBlock b) => new()
        {
            Date = a.Date,
            BlockNumber = (int)a.BlockNumber,
            TPS = (a.TransactionCount) / (a.Date.Subtract(b.Date).TotalSeconds),
            GPS = (a.GasUsed) / (a.Date.Subtract(b.Date).TotalSeconds),
            Provider = a.Provider,
        };
    }
}
