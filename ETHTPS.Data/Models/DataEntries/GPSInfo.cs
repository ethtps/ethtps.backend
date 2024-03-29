﻿using InfluxDB.Client.Core;

namespace ETHTPS.Data.Core.Models.DataEntries
{
    [Measurement("gps")]
    public sealed class GPSInfo : InfoBase
    {
        [Column("tps")]
        public double GPS { get; set; }
    }
}
