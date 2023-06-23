using System;
using System.Linq;
using System.Threading.Tasks;

using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater.TimeBuckets;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Models.DataUpdater;
using ETHTPS.Data.Integrations.InfluxIntegration;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Services.BlockchainServices.Extensions;
using ETHTPS.Services.BlockchainServices.HangfireLogging;

using Hangfire;

using Microsoft.Extensions.Logging;

namespace ETHTPS.Services.BlockchainServices.Status
{
    public abstract class HistoricalInfluxLogger<T> : BlockInfoProviderDataLoggerBase<T>
         where T : IHTTPBlockInfoProvider
    {
        private readonly IInfluxWrapper _influxWrapper;
        private readonly ITimeBucketDataUpdaterService<T>? _timeBucketService;

        private int _DELAY = 15 * 1000;
        public HistoricalInfluxLogger(T instance, ILogger<HangfireBackgroundService> logger, EthtpsContext context, IInfluxWrapper influxWrapper, IDataUpdaterStatusService statusService, ITimeBucketDataUpdaterService<T> timeBucketService = null) : base(instance, logger, context, statusService, UpdaterType.Historical)
        {
            _influxWrapper = influxWrapper;
            _timeBucketService = timeBucketService;
        }

        protected override string ServiceName { get => $"HistoricalInfluxLogger<{typeof(T).Name}>"; }

        private static bool _instanceRunning = false;
        private static object _lockObj = new object();
        [DisableConcurrentExecution(60 * 60 * 24 * 365)]
        public override async Task RunAsync()
        {
            lock (_lockObj)
            {
                if (!_instanceRunning)
                {
                    _instanceRunning = true;
                }
                else
                {
                    return;
                }
            }
            if (typeof(T).FullName.Contains("EthereumGenericJSONRPCBlockInfoProvider"))
                _DELAY = 10;
            if (!_context.OldestLoggedHistoricalEntries.Any(x => x.Network == _mainnetID && x.Provider == _providerID))
            {
                _context.OldestLoggedHistoricalEntries.Add(new OldestLoggedHistoricalEntry()
                {
                    Network = _mainnetID,
                    Provider = _providerID,
                    OldestBlock = (await _instance.GetLatestBlockInfoAsync()).BlockNumber
                });
                await _context.SaveChangesAsync();
            }
            var oldestEntry = _context.OldestLoggedHistoricalEntries.First(x => x.Network == _mainnetID && x.ProviderNavigation.Name == _provider);
            var step = 1;

            if (_context.Providers.Any(x => x.Id == _providerID && x.HistoricalAggregationDeltaBlock.HasValue))
            {
                step = _context.Providers.First(x => x.Id == _providerID).HistoricalAggregationDeltaBlock.Value;
            }

            int parallelQueriesCount = 25;

            while (oldestEntry.OldestBlock > 0)
            {
                var stopwatch = new System.Diagnostics.Stopwatch();
                try
                {
                    stopwatch.Restart();

                    _statusService.MarkAsRunning();
                    var tasks = Enumerable.Range(1, parallelQueriesCount).Select(i => Task.Run(async () => await _instance.GetBlockInfoAsync(oldestEntry.OldestBlock - i * step)));
                    var results = await Task.WhenAll(tasks);
                    if (results.Any(x => x == null))
                    {
                        _logger.LogInformation($"{ServiceName} - Null block(s) count: {string.Join(", ", results.Where(x => x == null).Count())}");
                        await Task.Delay(_DELAY);
                        continue;
                    }
                    results = results.OrderByDescending(x => x.Date).ToArray();
                    var list = results.ToList();
                    list.ForEach(block => block.Provider = _provider);
                    await _influxWrapper.LogBlocksAsync(list.ToArray());
                    var insertResult = await Task.WhenAll(list.Select(block => Task.Run(async () =>
                    {
                        if (_timeBucketService != null)
                        {
                            try
                            {
                                var delta = await CalculateTPSGPSAsync(block);
                                _timeBucketService?.UpdateAllEntries(delta);

                            }
                            catch (Exception e)
                            {
                                _logger.LogError($"{ServiceName} Error logging to MSSQL", e);
                                return false;
                            }
                        }
                        return true;
                    })));
                    stopwatch.Stop();
                    if (!insertResult.All(x => x))
                        throw new Exception($"Error logging data");
                    var dt = results.First().Date - results.Last().Date;
                    var eta = TimeSpan.FromMilliseconds(oldestEntry.OldestBlock * (stopwatch.Elapsed.TotalMilliseconds + _DELAY) / step);

                    oldestEntry.OldestBlock -= parallelQueriesCount * step;
                    oldestEntry.OldestBlockDate = list.Last().Date;
                    await _context.SaveChangesAsync(); //no gaps

                    _logger.LogInformation($"{ServiceName} - Logged blocks [#{list.First().BlockNumber}, ...,#{list.Last().BlockNumber}]\nDelta: -{dt.Hours}h {dt.Minutes}m {dt.Seconds}s\nETA: {eta.Days}d {eta.Hours}h {eta.Minutes}m\nCompleted: {Math.Round((double)(16607138 - oldestEntry.OldestBlock) * 100 / 16607138, 2)}%\nAverage speed: {Math.Round((double)parallelQueriesCount / stopwatch.Elapsed.TotalSeconds, 2)} req/s");
                }
                catch (Exception e)
                {
                    _logger.LogError($"HangfireHistoricalBlockInfoProviderDataLogger: {e.GetType().ToString()}", e.ToString());
                }
                await Task.Delay(_DELAY);
            }
        }
    }
}
