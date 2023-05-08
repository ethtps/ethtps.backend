using ETHTPS.Configuration;
using ETHTPS.Services.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Aurora")]
    [RunsEvery(CronConstants.EVERY_30_S)]
    public sealed class AuroraBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public AuroraBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Aurora")
        {
        }
    }
}
