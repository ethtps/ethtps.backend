using ETHTPS.Configuration;
using ETHTPS.Services.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Starknet")]
    public sealed class StarknetBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public StarknetBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Starknet")
        {
        }
    }
}
