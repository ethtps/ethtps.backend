
using ETHTPS.Services.Attributes;
using ETHTPS.Services.Ethereum.JSONRPC;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum
{
    [Provider("Habitat")]
    [Disabled]
    public sealed class HabitatBlockInfoProvider : JSONRPCBlockInfoProviderBase
    {
        public HabitatBlockInfoProvider(IConfiguration configuration) : base(configuration, "Habitat")
        {
        }
    }
}
