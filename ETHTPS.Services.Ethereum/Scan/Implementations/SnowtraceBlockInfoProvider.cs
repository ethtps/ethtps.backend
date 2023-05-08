using System;

using ETHTPS.Configuration;
using ETHTPS.Services.Attributes;

namespace ETHTPS.Services.Ethereum.Scan.Implementations
{
    [Provider("AVAX C-chain")]
    [Disabled]
    [Obsolete("Use JSONRPC.AVAXBlockInfoProvider instead", true)]
    public sealed class SnowTraceBlockInfoProvider : ScanBlockInfoProviderBase
    {
        public SnowTraceBlockInfoProvider(IDBConfigurationProvider configuration) : base(configuration, "Snowtrace")
        {
        }
    }
}
