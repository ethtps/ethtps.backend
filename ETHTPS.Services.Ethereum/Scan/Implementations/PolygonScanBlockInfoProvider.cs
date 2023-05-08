﻿using System;

using ETHTPS.Configuration;
using ETHTPS.Services.Attributes;

namespace ETHTPS.Services.Ethereum.Scan.Implementations
{
    [Provider("Polygon")]
    [Disabled]
    [Obsolete("Use JSONRPC.PolygonBlockInfoProvider instead", true)]
    public sealed class PolygonScanBlockInfoProvider : ScanBlockInfoProviderBase
    {
        public PolygonScanBlockInfoProvider(IDBConfigurationProvider configuration) : base(configuration, "Polygonscan")
        {
        }
    }
}
