using ETHTPS.Configuration;
using ETHTPS.Services.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Starknet")]
    [RunsEvery(CronConstants.EVERY_13_S)]
    public sealed class StarknetBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public StarknetBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Starknet")
        {
        }
    }
}
