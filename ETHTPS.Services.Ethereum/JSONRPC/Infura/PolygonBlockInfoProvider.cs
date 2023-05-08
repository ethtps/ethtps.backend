using ETHTPS.Services.Attributes;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Polygon")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class PolygonBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public PolygonBlockInfoProvider(IConfiguration configuration) : base(configuration, "PolygonEndpoint")
        {

        }
    }
}
