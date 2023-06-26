using System;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.Scan.Implementations
{
    [Provider("Polygon")]
    [Disabled]
    [Obsolete("Use JSONRPC.PolygonBlockInfoProvider instead", true)]
    public sealed class PolygonScanBlockInfoProvider : ScanBlockInfoProviderBase
    {
        public PolygonScanBlockInfoProvider(DBConfigurationProviderWithCache configuration) : base(configuration, "Polygonscan")
        {
        }
    }
}
