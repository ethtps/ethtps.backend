using ETHTPS.Services.Attributes;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Palm")]
    [RunsEvery(CronConstants.EVERY_30_S)]
    public class PalmBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public PalmBlockInfoProvider(IConfiguration configuration) : base(configuration, "PalmEndpoint")
        {

        }
    }
}
