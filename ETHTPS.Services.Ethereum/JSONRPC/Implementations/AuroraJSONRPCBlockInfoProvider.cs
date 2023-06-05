using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Implementations
{
    [Provider("Aurora")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class AuroraJSONRPCBlockInfoProvider : JSONRPCBlockInfoProviderBase
    {
        public AuroraJSONRPCBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Aurora")
        {
        }
    }
}
