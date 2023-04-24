using ETHTPS.Services.Attributes;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum.JSONRPC.Generic
{
    [Provider("Ethereum")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class EthereumGenericJSONRPCBlockInfoProvider : JSONRPCBlockInfoProviderBase
    {
        public EthereumGenericJSONRPCBlockInfoProvider(string endpoint) : base(endpoint)
        {

        }
        public EthereumGenericJSONRPCBlockInfoProvider(IConfiguration configuration) : base(configuration, "GenericJSONRPC", "Ethereum0")
        {
        }
    }
}
