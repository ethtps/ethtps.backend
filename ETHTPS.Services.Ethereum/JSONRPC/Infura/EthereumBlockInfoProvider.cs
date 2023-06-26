using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Services.BlockchainServices.BlockTime;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Ethereum")]
    [RunsEvery(CronConstants.EVERY_13_S)]
    public sealed class EthereumBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public EthereumBlockInfoProvider(DBConfigurationProviderWithCache configurationProvider, EthereumBlockTimeProvider ethereumBlockTimeProvider) : base(configurationProvider, "Ethereum")
        {
            BlockTimeSeconds = ethereumBlockTimeProvider.GetBlockTime();
        }
    }
}
