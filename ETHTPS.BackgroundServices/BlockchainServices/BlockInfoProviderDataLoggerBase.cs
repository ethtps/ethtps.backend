﻿using System;
using System.Linq;
using System.Threading.Tasks;

using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater.ProviderSpecific.TypeSpecific;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Data.Core.Models.DataUpdater;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Services.BlockchainServices.HangfireLogging;

using Microsoft.Extensions.Logging;

namespace ETHTPS.Services.BlockchainServices
{
    /// <summary>
    /// A base class for handling logic in order to transform block data into TPS/GPS data
    /// </summary>
    /// <typeparam name="T">Data provider type</typeparam>
    public abstract class BlockInfoProviderDataLoggerBase<T> : HangfireBackgroundService
        where T : IHTTPBlockInfoProvider
    {
        protected readonly T _instance;
        protected readonly string _provider;
        protected readonly int _providerID;
        protected readonly int _mainnetID;
        protected readonly IProviderTypeDataUpdaterStatusService _statusService;
        protected DateTime? LastRunTime
        {
            get => _statusService.GetLastRunTime();
        }

        protected TimeSpan? TimeSinceLastRan
        {
            get => _statusService.GetTimeSinceLastRan();
        }

        protected BlockInfoProviderDataLoggerBase(T instance, ILogger<HangfireBackgroundService> logger, EthtpsContext context, IDataUpdaterStatusService statusService, UpdaterType updaterType) : base(logger, context)
        {
            _instance = instance;
            _provider = _instance.GetProviderName();
            _providerID = _context.Providers.First(x => x.Name == _provider).Id;

            _mainnetID = context.Networks.First(x => x.Name == "Mainnet").Id;
            _statusService = statusService.MakeProviderSpecific(_provider).MakeUpdaterSpecific(updaterType);
        }

        protected async Task<TPSGPSInfo> CalculateTPSGPSAsync() => await CalculateTPSGPSAsync(await _instance.GetLatestBlockInfoAsync());
        protected async Task<TPSGPSInfo> CalculateTPSGPSAsync(int blockNumber) => await CalculateTPSGPSAsync(await _instance.GetBlockInfoAsync(blockNumber));
        protected async Task<TPSGPSInfo> CalculateTPSGPSAsync(DateTime atTime) => await CalculateTPSGPSAsync(await _instance.GetBlockInfoAsync(atTime));
        protected async Task<TPSGPSInfo> CalculateTPSGPSAsync(Block latestBlock)
        {
            if (latestBlock == null)
            {
                _logger?.LogWarning($"Latest block was null for {_provider}");
                return null;
            }
            if (_instance.BlockTimeSeconds != null && _instance.BlockTimeSeconds > 0)
            {
                return new TPSGPSInfo()
                {
                    BlockNumber = latestBlock.BlockNumber,
                    Date = latestBlock.Date,
                    GPS = latestBlock.GasUsed / (double)_instance.BlockTimeSeconds,
                    TPS = latestBlock.TransactionCount / (double)_instance.BlockTimeSeconds,
                    Provider = _provider,
                    TransactionCount = latestBlock.TransactionCount,
                    GasUsed = latestBlock.GasUsed,
                    TransactionHashes = latestBlock.TXHashes
                };
            }
            else //Add up all blocks submitted at the same time
            {
                TPSGPSInfo result = new()
                {
                    Date = latestBlock.Date,
                    Provider = _provider,
                    BlockNumber = latestBlock.BlockNumber,
                    TransactionHashes = latestBlock.TXHashes
                };
                Block secondToLatestBlock;
                int count = 0;
                do
                {
                    result.TPS += latestBlock.TransactionCount;
                    result.GPS += latestBlock.GasUsed;

                    secondToLatestBlock = await _instance.GetBlockInfoAsync(latestBlock.BlockNumber - 1);
                    if (secondToLatestBlock.Date.Subtract(latestBlock.Date).TotalSeconds != 0)
                    {
                        result.TPS /= Math.Abs(secondToLatestBlock.Date.Subtract(result.Date).TotalSeconds);
                        result.GPS /= Math.Abs(secondToLatestBlock.Date.Subtract(result.Date).TotalSeconds);
                        break;
                    }
                    if (secondToLatestBlock.TXHashes?.Length > 0)
                    {
                        result.TransactionHashes = result.TransactionHashes.Concat(secondToLatestBlock.TXHashes).ToArray();
                    }
                    latestBlock = secondToLatestBlock;
                    await Task.Delay(200);
                    if (++count == 100)
                    {
                        throw new Exception($"Possible infinite loop {typeof(T)}");
                    }
                }
                while (true);
                return result;
            }
        }
    }
}
