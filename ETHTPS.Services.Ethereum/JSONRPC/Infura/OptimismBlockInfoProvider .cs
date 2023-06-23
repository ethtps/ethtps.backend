using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Optimism")]
    [RunsEvery(CronConstants.EVERY_13_S)]
    public sealed class OptimismBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public OptimismBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Optimism")
        {
        }
    }
}
