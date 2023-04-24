using System;
using System.Net.Http;
using System.Threading.Tasks;

using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Services.Attributes;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum
{
    [Provider("ZKSync")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public class ZKSsyncBlockInfoProvider : IHTTPBlockInfoProvider
    {
        private readonly HttpClient _httpClient;
        public ZKSsyncBlockInfoProvider()
        {
            _httpClient = new HttpClient();
        }

        public double BlockTimeSeconds { get; set; }

        public async Task<Block> GetBlockInfoAsync(int blockNumber)
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

        public Task<Block> GetBlockInfoAsync(DateTime time)
        {
            throw new NotImplementedException();
        }

        public async Task<Block> GetLatestBlockInfoAsync()
        {
            var blocks = JsonConvert.DeserializeObject<dynamic>(await _httpClient.GetStringAsync("https://api.zksync.io/api/v0.1/blocks?limit=2"));
            return await GetBlockInfoAsync(int.Parse(blocks[0].block_number.ToString()));
        }
    }
}
