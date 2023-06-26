using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.Scan.Implementations
{
    [Provider("Fantom")]
    [RunsEvery(CronConstants.EVERY_10_S)]
    public sealed class FTMScanBlockInfoProvider : ScanBlockInfoProviderBase
    {
        public FTMScanBlockInfoProvider(DBConfigurationProviderWithCache configuration) : base(configuration, "Fantom")
        {
        }
    }
}
