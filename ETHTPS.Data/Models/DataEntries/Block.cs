using System;

using InfluxDB.Client.Core;

namespace ETHTPS.Data.Core.Models.DataEntries
{
    [Measurement("blockinfo")]
    public sealed class Block : IBlock
    {
        [Column("blocknumber")]
        public int BlockNumber { get; set; }
        [Column("transactioncount")]
        public int TransactionCount { get; set; }
        [Column("gasused")]
        public int GasUsed { get; set; }
        public DateTime Date { get; set; }
        [Column("settled")]
        public bool Settled { get; set; } = true;
        [Column("provider", IsTag = true)]
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
        public InfluxBlock ToInfluxBlock() =>
            new()
            {
                BlockNumber = BlockNumber,
                TransactionCount = TransactionCount,
                GasUsed = GasUsed,
                Provider = Provider,
                TXHashes = TXHashes,
            };
    }

    /// <summary>
    /// A double-based implementation of <see cref="IBlock"/> since Influx uses doubles instead of ints. Note: this class can only be used for **retrieving** objects from the database. For writing, use a <see cref="Block"/> instance; you can create one using the <see cref="ToBlock"/> method.
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
