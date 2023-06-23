using System;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.Models.DataEntries;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum
{
    [Provider("ZKSync")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class ZKSsyncBlockInfoProvider : BlockInfoProviderBase
    {
        public ZKSsyncBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "ZKSync")
        {

        }

        public override async Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            var obj = JsonConvert.DeserializeObject<dynamic>(await _httpClient.GetStringAsync($"https://api.zksync.io/api/v0.1/blocks/{blockNumber}"));
            var txs = JsonConvert.DeserializeObject<dynamic>(await _httpClient.GetStringAsync($"https://api.zksync.io/api/v0.1/blocks/{blockNumber}/transactions"));
            var block = obj;
            return new Block()
            {
                Date = DateTime.Parse(block.committed_at.ToString()),
                BlockNumber = int.Parse(block.block_number.ToString()),
                TransactionCount = txs.Count
            };
        }

        public override Task<Block> GetBlockInfoAsync(DateTime time)
        {
            throw new NotImplementedException();
        }

        public override async Task<Block> GetLatestBlockInfoAsync()
        {
            var blocks = JsonConvert.DeserializeObject<dynamic>(await _httpClient.GetStringAsync("https://api.zksync.io/api/v0.1/blocks?limit=2"));
            return await GetBlockInfoAsync(int.Parse(blocks[0].block_number.ToString()));
        }
    }
}
