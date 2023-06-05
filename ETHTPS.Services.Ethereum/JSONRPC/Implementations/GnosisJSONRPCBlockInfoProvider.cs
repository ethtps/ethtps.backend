using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Implementations
{
    [Provider("Gnosis")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class GnosisJSONRPCBlockInfoProvider : JSONRPCBlockInfoProviderBase
    {
        public GnosisJSONRPCBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Gnosis")
        {
        }
    }
}
