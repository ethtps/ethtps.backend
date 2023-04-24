using ETHTPS.Services.Attributes;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Arbitrum One")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class ArbitrumBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public ArbitrumBlockInfoProvider(IConfiguration configuration) : base(configuration, "ArbitrumEndpoint")
        {

        }
    }
}
