using ETHTPS.Configuration;
using ETHTPS.Services.Attributes;
using ETHTPS.Services.BlockchainServices.BlockTime;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Ethereum")]
    [RunsEvery(CronConstants.EVERY_13_S)]
    public sealed class EthereumBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public EthereumBlockInfoProvider(IDBConfigurationProvider configurationProvider, EthereumBlockTimeProvider ethereumBlockTimeProvider) : base(configurationProvider, "Ethereum")
        {
            BlockTimeSeconds = ethereumBlockTimeProvider.GetBlockTime();
        }
    }
}
