using System;

using ETHTPS.Configuration;
using ETHTPS.Services.Attributes;

namespace ETHTPS.Services.Ethereum.Scan.Implementations
{
    [Provider("Arbitrum One")]
    [Obsolete("Use JSONRPC.PolygonBlockInfoProvider instead", true)]
    [Disabled]
    public sealed class ArbiscanBlockInfoProvider : ScanBlockInfoProviderBase
    {
        public ArbiscanBlockInfoProvider(IDBConfigurationProvider configuration) : base(configuration, "Arbiscan")
        {
        }
    }
}
