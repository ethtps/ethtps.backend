using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.Scan.Implementations
{
    [Provider("Binance Smart Chain")]
    [Disabled]
    public sealed class BSCScanBlockInfoProvider : ScanBlockInfoProviderBase
    {
        public BSCScanBlockInfoProvider(IDBConfigurationProvider configuration) : base(configuration, "BSCScan")
        {
        }
    }
}
