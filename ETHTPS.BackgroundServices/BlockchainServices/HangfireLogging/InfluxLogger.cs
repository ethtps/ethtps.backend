using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.Core;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Data.Core.Models.DataUpdater;
using ETHTPS.Data.Core.Models.LiveData;
using ETHTPS.Data.Core.Models.LiveData.Triggers;
using ETHTPS.Data.Integrations.InfluxIntegration;
using ETHTPS.Data.Integrations.InfluxIntegration.ProviderServices;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Services.BlockchainServices.Extensions;
using ETHTPS.Services.LiveData;

using Hangfire;

using Microsoft.Extensions.Logging;

namespace ETHTPS.Services.BlockchainServices.HangfireLogging
{
    public class InfluxLogger<T> : BlockInfoProviderDataLoggerBase<T>
         where T : IHTTPBlockInfoProvider
    {
        private static IBucketCreator _bucketCreator;
        private static object _lockObj = new object();
        private static Dictionary<string, int> _lastBlockNumberDictionary = new();
        private readonly IInfluxWrapper _influxWrapper;
        private TimeSpan _timeout = TimeSpan.FromSeconds(30);
        private readonly WSAPIPublisher _wsapiClient;
        private readonly IRedisCacheService _redisCacheService;
        protected override string ServiceName { get => $"InfluxLogger<{typeof(T).Name}>"; }
        public InfluxLogger(T instance, ILogger<HangfireBackgroundService> logger, EthtpsContext context, IInfluxWrapper influxWrapper, IDataUpdaterStatusService statusService, WSAPIPublisher wsapiClient, IRedisCacheService redisCacheService) : base(instance, logger, context, statusService, UpdaterType.BlockInfo)
        {
            _influxWrapper = influxWrapper;
            _bucketCreator ??= new MeasurementBucketCreator(influxWrapper);
            _wsapiClient = wsapiClient;
            _redisCacheService = redisCacheService;
        }

        [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        //        [DisableConcurrentExecution(15)]
        public override async Task RunAsync()
        {
            if (!_statusService.Enabled ?? false)
            {
                _logger.LogInformation($"Not running InfluxLogger<{_provider}> because it is disabled");
                return; //TODO: remove self from hangfire
            }
            if (TimeSinceLastRan?.TotalSeconds >= 5)
            {
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

                    await CreateBucketsIfNeededAsync();

                    _statusService.SetStatusFor(UpdaterType.BlockInfo, UpdaterStatus.Running);
                    var block = await _instance.GetLatestBlockInfoAsync();
                    if (block != null)
                    {
                        block.Provider = _provider;
                        if (ShouldSkipBlock(block))
                        {
                            _logger.LogInformation($"Skipping {ServiceName} run because block {block} was already logged");
                            _statusService.MarkAsRanSuccessfully();
                            return;
                        }
                        await _influxWrapper.LogBlockAsync(block);
                        TPSGPSInfo delta = await CalculateTPSGPSAsync(block);
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
                        await _redisCacheService.SetDataAsync(new L2DataUpdateModel()
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
                            }),
                            CacheKey = $"Instant_{_provider}"
                        });
                        var maxKey = $"Max_{_provider}";
                        if (await _redisCacheService.HasKeyAsync(maxKey))
                        {
                            var max = await _redisCacheService.GetDataAsync<MinimalDataPoint>(maxKey);
                            max.TPS = Math.Max(max.TPS ?? 0, delta.TPS);
                            max.GPS = Math.Max(max.GPS ?? 0, delta.GPS);
                            await _redisCacheService.SetDataAsync(maxKey, max);
                        }
                        else
                        {
                            await _redisCacheService.SetDataAsync(maxKey, new MinimalDataPoint()
                            {
                                TPS = delta.TPS,
                                GPS = delta.GPS
                            });
                        }
                        _logger.LogInformation($"{_provider}: {delta.TPS}TPS {delta.GPS}GPS");

                        _statusService.SetStatusFor(UpdaterType.BlockInfo, UpdaterStatus.RanSuccessfully);
                    }
                    else
                    {
                        _logger.LogError($"{ServiceName}: no data returned");
                        _statusService.MarkAsFailed();
                    }
                }
                catch (InfluxException e)
                {
                    _statusService.MarkAsFailed();
                    _logger.LogError(new EventId(), e, $"InfluxLogger exception running InfluxLogger<{typeof(T)}>");
                    throw;
                }
                catch (Exception e)
                {
                    _logger.LogError(new EventId(), e, $"Exception running InfluxLogger<{typeof(T)}>");
                    _statusService.SetStatusFor(UpdaterType.BlockInfo, UpdaterStatus.Failed);
                }
            }
            else
            {
                _logger.LogTrace($"Skipping {ServiceName} run because it was reran too quickly");
            }
        }
        private static bool ShouldSkipBlock(Block blockInfo) => ShouldSkipBlock(blockInfo.Provider, blockInfo.BlockNumber);
        private static bool ShouldSkipBlock(string provider, int block)
        {
            lock (_lockObj)
            {
                if (!_lastBlockNumberDictionary.ContainsKey(provider))
                {
                    _lastBlockNumberDictionary.Add(provider, block);
                    return false;
                }
                if (_lastBlockNumberDictionary[provider] == block)
                    return true;
                _lastBlockNumberDictionary[provider] = block;
                return false;
            }
        }

        private static async Task CreateBucketsIfNeededAsync()
        {
            if (!_bucketCreator.Created)
            {
                await _bucketCreator.CreateBucketsAsync();
            }
        }
    }
}
