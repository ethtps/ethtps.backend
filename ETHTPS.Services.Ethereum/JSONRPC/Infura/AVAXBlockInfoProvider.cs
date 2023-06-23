using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("AVAX C-chain")]
    [RunsEvery(CronConstants.EVERY_30_S)]
    public sealed class AVAXBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public AVAXBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "AVAX C-chain")
        {

        }
    }
}
