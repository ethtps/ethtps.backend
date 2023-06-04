
using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Services.Ethereum.JSONRPC;

namespace ETHTPS.Services.Ethereum
{
    [Provider("Habitat")]
    [Disabled]
    public sealed class HabitatBlockInfoProvider : JSONRPCBlockInfoProviderBase
    {
        public HabitatBlockInfoProvider(IDBConfigurationProvider configuration) : base(configuration, "Habitat")
        {
        }
    }
}
