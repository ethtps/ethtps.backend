using ETHTPS.Configuration;
using ETHTPS.Services.Attributes;

namespace ETHTPS.Services.Ethereum.Scan.Implementations
{
    [Provider("Fantom")]
    [RunsEvery(CronConstants.EVERY_10_S)]
    public sealed class FTMScanBlockInfoProvider : ScanBlockInfoProviderBase
    {
        public FTMScanBlockInfoProvider(IDBConfigurationProvider configuration) : base(configuration, "FTMScan")
        {
        }
    }
}
