using System;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.Scan.Implementations
{
    [Provider("Ethereum")]
    [Disabled]
    [Obsolete("Use JSONRPC.EthereumBlockInfoProvider instead", true)]
    public sealed class EtherscanBlockInfoProvider : ScanBlockInfoProviderBase
    {
        public EtherscanBlockInfoProvider(DBConfigurationProviderWithCache configuration) : base(configuration, "Etherscan")
        {

        }
    }
}
