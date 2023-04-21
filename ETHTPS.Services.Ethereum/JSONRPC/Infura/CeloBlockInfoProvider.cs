using ETHTPS.Services.Attributes;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Celo")]
    [RunsEvery(CronConstants.EVERY_30_S)]
    public class CeloBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public CeloBlockInfoProvider(IConfiguration configuration) : base(configuration, "CeloEndpoint")
        {

        }
    }
}
