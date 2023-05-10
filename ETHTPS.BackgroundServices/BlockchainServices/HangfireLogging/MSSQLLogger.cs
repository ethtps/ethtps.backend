using System;
using System.Threading.Tasks;

using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater.TimeBuckets;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Models.DataEntries.BlockchainServices.Models;
using ETHTPS.Data.Core.Models.DataUpdater;
using ETHTPS.Data.Integrations.MSSQL;

using Hangfire;

using Microsoft.Extensions.Logging;

namespace ETHTPS.Services.BlockchainServices.HangfireLogging
{
    public class MSSQLLogger<T> : BlockInfoProviderDataLoggerBase<T>
         where T : IHTTPBlockInfoProvider
    {
        protected override string ServiceName { get => $"MSSQLLogger<{typeof(T).Name}>"; }
        protected readonly ITimeBucketDataUpdaterService<T> _timeBucketService;
        private TimeSpan _timeout = TimeSpan.FromSeconds(30);
        public MSSQLLogger(T instance,
                           ILogger<HangfireBackgroundService> logger,
                           EthtpsContext context,
                           IDataUpdaterStatusService statusService,
                           ITimeBucketDataUpdaterService<T> timeBucketService)
            : base(instance, logger, context, statusService, UpdaterType.TPSGPS)
        {
            _timeBucketService = timeBucketService;
        }

        [AutomaticRetry(Attempts = 1, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public override async Task RunAsync()
        {
            if (_statusService.Enabled ?? false)
            {
                return;
            }
            try
            {
                if (DateTime.Now - _statusService.GetLastRunTime() < _timeout)
                {
                    if (_statusService.GetStatusFor(UpdaterType.BlockInfo) == UpdaterStatus.Running)
                    {
                        _logger.LogWarning($"{_provider}: Updater is already running");
                        return;
                    }
                }
                _statusService.SetStatusFor(UpdaterType.BlockInfo, UpdaterStatus.Running);

                TPSGPSInfo delta = await CalculateTPSGPSAsync();
                if (delta == null) return;
                _timeBucketService.UpdateAllEntries(delta);
                await _context.SaveChangesAsync();
                await _context.TryCreateNewBlockSummaryAsync(delta);
                _logger.LogInformation($"{_provider}: {delta.TPS}TPS {delta.GPS}GPS");

                _statusService.SetStatusFor(UpdaterType.BlockInfo, UpdaterStatus.RanSuccessfully);
            }
            catch (Exception e)
            {
                _logger.LogError("MSSQLLogger", e);
                _statusService.SetStatusFor(UpdaterType.BlockInfo, UpdaterStatus.Failed);
            }
        }
#if DEBUG
        public async Task RunWithoutExceptionHandlingAsync()
        {
            _logger.LogInformation($"RunWithoutExceptionHandlingAsync({_provider})");
            _statusService.SetStatusFor(UpdaterType.BlockInfo, UpdaterStatus.Running);

            TPSGPSInfo delta = await CalculateTPSGPSAsync() ?? throw new Exception("Delta is null");
            _logger.LogInformation($"delta.Provider: {delta.Provider}");
            _timeBucketService.UpdateAllEntries(delta);
            await _context.SaveChangesAsync();
            await _context.TryCreateNewBlockSummaryAsync(delta);
            _logger.LogInformation($"{_provider}: {delta.TPS}TPS {delta.GPS}GPS");

            _statusService.SetStatusFor(UpdaterType.BlockInfo, UpdaterStatus.RanSuccessfully);
        }
#endif
    }
}
