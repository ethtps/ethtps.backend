using System;
using System.Linq;
using System.Threading.Tasks;

using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater.TimeBuckets;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Data.Core.Models.DataUpdater;
using ETHTPS.Data.Core.Models.LiveData;
using ETHTPS.Data.Core.Models.LiveData.Triggers;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Services.LiveData;

using Hangfire;

using Microsoft.Extensions.Logging;

namespace ETHTPS.Services.BlockchainServices.HangfireLogging
{
    /// <summary>
    /// A data logger that logs data from a block info provider to an MSSQL database
    /// </summary>
    /// <typeparam name="T">Data provider type</typeparam>
    public class MSSQLLogger<T> : BlockInfoProviderDataLoggerBase<T>
         where T : IHTTPBlockInfoProvider
    {
        protected override string ServiceName { get => $"MSSQLLogger<{typeof(T).Name}>"; }
        protected readonly ITimeBucketDataUpdaterService<T> _timeBucketService;
        private readonly WSAPIPublisher _wsapiClient;
        private TimeSpan _timeout = TimeSpan.FromSeconds(30);
        public MSSQLLogger(T instance,
                           ILogger<HangfireBackgroundService> logger,
                           EthtpsContext context,
                           IDataUpdaterStatusService statusService,
                           ITimeBucketDataUpdaterService<T> timeBucketService,
                           WSAPIPublisher wsapiClient)
            : base(instance, logger, context, statusService, UpdaterType.TPSGPS)
        {
            _timeBucketService = timeBucketService;
            _wsapiClient = wsapiClient;
        }

        /// <summary>
        /// Gathers data from the provider and logs it to the database
        /// </summary>
        [AutomaticRetry(Attempts = 1, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public override async Task RunAsync()
        {
            if (!_statusService.Enabled ?? false)
            {
                _logger.LogInformation($"Not running MSSQLLogger<{_provider}> because it is disabled");
                return; //TODO: remove self from hangfire
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

                TPSGPSInfo delta = await CalculateTPSGPSAsync() ?? throw new ArgumentNullException($"MSSQLLogger<{_provider}> returned null");
                _timeBucketService.UpdateAllEntries(delta);
                await _context.SaveChangesAsync();
                await _context.TryCreateNewBlockSummaryAsync(delta);
                _wsapiClient.Push(new L2DataUpdateModel()
                {
                    BlockNumber = delta.BlockNumber,
                    Data = new MinimalDataPoint()
                    {
                        GPS = delta.GPS,
                        TPS = delta.TPS
                    },
                    Provider = _provider,
                    Transactions = delta.TransactionHashes?.Select(x => new TransactionMetadata()
                    {
                        Hash = x
                    })
                });
                _logger.LogInformation($"{_provider}: {delta.TPS}TPS {delta.GPS}GPS");

                _statusService.SetStatusFor(UpdaterType.BlockInfo, UpdaterStatus.RanSuccessfully);
            }
            catch (Exception e)
            {
                _logger.LogError("MSSQLLogger", e.ToString());
                _statusService.SetStatusFor(UpdaterType.BlockInfo, UpdaterStatus.Failed);
            }
        }

        /// <summary>
        /// This method is used to run the updater without exception handling - only to be used for testing purposes
        /// </summary>
        /// <exception cref="Exception"></exception>
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
    }
}
