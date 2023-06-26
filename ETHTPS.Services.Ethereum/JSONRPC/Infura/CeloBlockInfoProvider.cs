using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Celo")]
    [RunsEvery(CronConstants.EVERY_30_S)]
    public sealed class CeloBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public CeloBlockInfoProvider(DBConfigurationProviderWithCache configurationProvider) : base(configurationProvider, "Celo")
        {
        }
    }
}
