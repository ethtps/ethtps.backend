using ETHTPS.Services.Attributes;
using ETHTPS.Services.BlockchainServices;
using ETHTPS.Services.BlockchainServices.BlockTime;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Arbitrum One")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public class ArbitrumBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public ArbitrumBlockInfoProvider(IConfiguration configuration) : base(configuration, "ArbitrumEndpoint")
        {

        }
    }
}
