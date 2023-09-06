using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Polygon")]
    [RunsEvery(CronConstants.EVERY_13_S)]
    public sealed class PolygonBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public PolygonBlockInfoProvider(DBConfigurationProviderWithCache configurationProvider) : base(configurationProvider, "Polygon")
        {
        }
    }
}
