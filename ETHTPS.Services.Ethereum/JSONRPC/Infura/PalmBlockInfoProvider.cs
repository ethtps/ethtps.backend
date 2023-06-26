using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Palm")]
    [RunsEvery(CronConstants.EVERY_30_S)]
    public sealed class PalmBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public PalmBlockInfoProvider(DBConfigurationProviderWithCache configurationProvider) : base(configurationProvider, "Palm")
        {
        }
    }
}
