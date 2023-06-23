using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Services.Ethereum.JSONRPC;

namespace ETHTPS.Services.Ethereum
{
    [Provider("Polygon")]
    public sealed class PolygonBlockInfoProvider : JSONRPCBlockInfoProviderBase, IHTTPBlockInfoProvider
    {
        public PolygonBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Polygon")
        {
        }
    }
}
