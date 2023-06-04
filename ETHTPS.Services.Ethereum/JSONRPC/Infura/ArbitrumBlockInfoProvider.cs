using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Arbitrum One")]
    [RunsEvery(CronConstants.EVERY_13_S)]
    public sealed class ArbitrumBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public ArbitrumBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Arbitrum One")
        {
        }
    }
}
