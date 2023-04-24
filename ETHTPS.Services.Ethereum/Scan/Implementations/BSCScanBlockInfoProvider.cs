using ETHTPS.Services.Attributes;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum.Scan.Implementations
{
    [Provider("Binance Smart Chain")]
    [Disabled]
    public sealed class BSCScanBlockInfoProvider : ScanBlockInfoProviderBase
    {
        public BSCScanBlockInfoProvider(IConfiguration configuration) : base(configuration, "BSCScan")
        {
        }
    }
}
