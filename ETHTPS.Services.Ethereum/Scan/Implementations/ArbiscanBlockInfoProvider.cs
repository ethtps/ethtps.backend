using System;

using ETHTPS.Services.Attributes;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum.Scan.Implementations
{
    [Provider("Arbitrum One")]
    [Obsolete("Use JSONRPC.PolygonBlockInfoProvider instead", true)]
    [Disabled]
    public sealed class ArbiscanBlockInfoProvider : ScanBlockInfoProviderBase
    {
        public ArbiscanBlockInfoProvider(IConfiguration configuration) : base(configuration, "Arbiscan")
        {
        }
    }
}
