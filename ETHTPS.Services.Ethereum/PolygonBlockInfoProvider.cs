using ETHTPS.API.BIL.Infrastructure.Services.BlockInfo;
using ETHTPS.Services.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC
{
    [Provider("Polygon")]
    public class PolygonBlockInfoProvider : JSONRPCBlockInfoProviderBase, IHTTPBlockInfoProvider
    {
        public PolygonBlockInfoProvider(string endpoint) : base(endpoint)
        {
        }
    }
}
