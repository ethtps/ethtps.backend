using ETHTPS.Configuration;
using ETHTPS.Services.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Arbitrum One")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class ArbitrumBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public ArbitrumBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Arbitrum One")
        {
        }
    }
}
