using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Implementations
{
    [Provider("Optimism")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class OptimismJSONRPCBlockInfoProvider : JSONRPCBlockInfoProviderBase
    {
        public OptimismJSONRPCBlockInfoProvider(DBConfigurationProviderWithCache configurationProvider) : base(configurationProvider, "Optimism")
        {
        }
    }
}
