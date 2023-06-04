using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Generic
{
    [Provider("Ethereum")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class EthereumGenericJSONRPCBlockInfoProvider : JSONRPCBlockInfoProviderBase
    {
        public EthereumGenericJSONRPCBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Ethereum")
        {
        }
    }
}
