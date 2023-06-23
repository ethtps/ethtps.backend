using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater.TimeBuckets;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Models.DataEntries;

using Microsoft.Extensions.Logging;

namespace ETHTPS.Data.Integrations.InfluxIntegration
{
    public sealed class InfluxTimeBucketService<T> : ITimeBucketDataUpdaterService<T>
         where T : IHTTPBlockInfoProvider
    {
        private readonly ILogger<InfluxTimeBucketService<T>> _logger;

        public InfluxTimeBucketService(ILogger<InfluxTimeBucketService<T>> logger)
        {
            _logger = logger;
        }

        public void AddOrUpdateAllTPSEntry(TPSGPSInfo entry)
        {
            _logger.LogInformation("AddOrUpdateAllTPSEntry");
        }

        public void AddOrUpdateDayTPSEntry(TPSGPSInfo entry)
        {
            _logger.LogInformation("AddOrUpdateDayTPSEntry");
        }

        public void AddOrUpdateHourTPSEntry(TPSGPSInfo entry)
        {
            _logger.LogInformation("AddOrUpdateHourTPSEntry");
        }

        public void AddOrUpdateMinuteTPSEntry(TPSGPSInfo entry)
        {
            _logger.LogInformation("AddOrUpdateMinuteTPSEntry");
        }

        public void AddOrUpdateMonthTPSEntry(TPSGPSInfo entry)
        {
            _logger.LogInformation("AddOrUpdateMonthTPSEntry");
        }

        public void AddOrUpdateWeekTPSEntry(TPSGPSInfo entry)
        {
            _logger.LogInformation("AddOrUpdateWeekTPSEntry");
        }

        public void AddOrUpdateYearTPSEntry(TPSGPSInfo entry)
        {
            _logger.LogInformation("AddOrUpdateYearTPSEntry");
        }

        public void UpdateAllEntries(TPSGPSInfo entry)
        {
            _logger.LogInformation("UpdateAllEntries");
        }

        public void UpdateLatestEntries(TPSGPSInfo entry)
        {
            _logger.LogInformation("UpdateLatestEntries");
        }

        public void UpdateMaxEntry(TPSGPSInfo entry)
        {
            _logger.LogInformation("UpdateMaxEntry");
        }
    }
}
