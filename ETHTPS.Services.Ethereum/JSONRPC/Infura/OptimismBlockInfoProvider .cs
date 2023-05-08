using ETHTPS.Configuration;
using ETHTPS.Services.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Optimism")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class OptimismBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public OptimismBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Optimism")
        {
        }
    }
}
