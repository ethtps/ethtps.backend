using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("NEAR")]
    [RunsEvery(CronConstants.EVERY_30_S)]
    public sealed class NEARBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public NEARBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "NEAR")
        {
        }
    }
}
