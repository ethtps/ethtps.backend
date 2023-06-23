using System;
using System.Linq;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Data.Integrations.MSSQL.Extensions;
using ETHTPS.Services.Ethereum.Starkware.API;

namespace ETHTPS.Services.Ethereum.Starkware
{
    [RunsEvery(CronConstants.EVERY_30_S)]
    public abstract class StarkwareBlockInfoProviderBase : BlockInfoProviderBase
    {
        private readonly EthtpsContext _context;
        private readonly StarkwareClient _starkwareClient;

        protected StarkwareBlockInfoProviderBase(string productName, EthtpsContext context, IDBConfigurationProvider configuration) : base(configuration, productName)
        {
            _providerName = productName;
            _context = context;

            _starkwareClient = new StarkwareClient(Endpoint, APIKey);
            BlockTimeSeconds = 100;
        }

        public override Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            throw new NotImplementedException();
        }

        public override async Task<Block> GetBlockInfoAsync(DateTime time)
        {
            var txCountForDay = await _starkwareClient.GetTransactionCountForAllTokensAsync(time, _providerName);
            return new Block()
            {
                Date = time,
                Settled = true,
                TransactionCount = (int)(100 * txCountForDay / 86400)
            };
        }

        public override async Task<Block> GetLatestBlockInfoAsync()
        {
            var todaysTransactionCount = await _starkwareClient.GetTodayTransactionCountForAllTokensAsync(_providerName);
            var mainnetID = _context.GetMainnetID();
            if (!_context.StarkwareTransactionCountData.Any(x => x.Product == _providerName)) //First time we see this product
            {
                _context.StarkwareTransactionCountData.Add(new StarkwareTransactionCountDatum()
                {
                    LastUpdateCount = todaysTransactionCount,
                    LastUpdateTime = DateTime.Now,
                    Network = mainnetID,
                    Product = _providerName,
                    LastUpdateTps = todaysTransactionCount / DateTime.Now.TimeOfDay.TotalSeconds
                });
                await _context.SaveChangesAsync();
            }

            var entry = _context.StarkwareTransactionCountData.First(x => x.Product == _providerName);
            if (entry.LastUpdateCount != todaysTransactionCount) //tx count has changed, update the entry
            {
                if (entry.LastUpdateTime.Day == DateTime.Now.Day)
                {
                    entry.LastUpdateTps = (todaysTransactionCount - entry.LastUpdateCount) / DateTime.Now.Subtract(entry.LastUpdateTime).TotalSeconds;  //TPS since last update
                }
                else //New day
                {
                    entry.LastUpdateTps = (double)todaysTransactionCount / 86400;
                }

                entry.LastUpdateCount = todaysTransactionCount;
                entry.LastUpdateTime = DateTime.Now;

                _context.StarkwareTransactionCountData.Update(entry);
                await _context.SaveChangesAsync();
            }
            return new Block()
            {
                Settled = true,
                TransactionCount = (int)(100 * entry.LastUpdateTps),
                Date = DateTime.Now
            };
        }
    }
}
