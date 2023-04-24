using ETHTPS.Services.Attributes;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Optimism")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class OptimismBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public OptimismBlockInfoProvider(IConfiguration configuration) : base(configuration, "OptimismEndpoint")
        {

        }
    }
}
