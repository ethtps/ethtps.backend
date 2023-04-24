using ETHTPS.Services.Attributes;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Starknet")]
    public sealed class StarknetBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public StarknetBlockInfoProvider(IConfiguration configuration) : base(configuration, "StarknetEndpoint")
        {

        }
    }
}
