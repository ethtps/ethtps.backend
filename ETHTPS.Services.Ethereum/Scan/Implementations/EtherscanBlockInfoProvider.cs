using System;

using ETHTPS.Services.Attributes;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum.Scan.Implementations
{
    [Provider("Ethereum")]
    [Disabled]
    [Obsolete("Use JSONRPC.EthereumBlockInfoProvider instead", true)]
    public sealed class EtherscanBlockInfoProvider : ScanBlockInfoProviderBase
    {
        public EtherscanBlockInfoProvider(IConfiguration configuration) : base(configuration, "Etherscan")
        {

        }
    }
}
