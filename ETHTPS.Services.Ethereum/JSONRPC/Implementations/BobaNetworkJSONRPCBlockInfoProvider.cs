using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Implementations
{
    [Provider("Boba Network")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class BobaNetworkJSONRPCBlockInfoProvider : JSONRPCBlockInfoProviderBase
    {
        public BobaNetworkJSONRPCBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Boba Network")
        {
        }
    }
}
