using System;
using System.Net.Http;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Extensions;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Services.Attributes;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum
{
    [Provider("ZKSwap")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class ZKSwapBlockInfoProvider : BlockInfoProviderBase
    {
        private readonly HttpClient _httpClient;
        public ZKSwapBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "ZKSwap")
        {
            _httpClient = new HttpClient();
        }
        public double BlockTimeSeconds { get; set; }

        public override async Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            var obj = JsonConvert.DeserializeObject<dynamic>(await _httpClient.GetStringAsync($"https://api.zkswap.info/v2/block/{blockNumber}"));
            var block = obj.data;
            return new Block()
            {
                Date = DateTimeExtensions.FromUnixTime(long.Parse(block.committed_at.ToString())),
                BlockNumber = int.Parse(block.number.ToString()),
                TransactionCount = int.Parse(block.transactions_number.ToString())
            };
        }

        public override Task<Block> GetBlockInfoAsync(DateTime time)
        {
            throw new NotImplementedException();
        }

        public override async Task<Block> GetLatestBlockInfoAsync()
        {
            var blocks = JsonConvert.DeserializeObject<dynamic>(await _httpClient.GetStringAsync("https://api.zkswap.info/v2/blocks?start=0&limit=2"));
            return await GetBlockInfoAsync(int.Parse(blocks.data.data[0].number.ToString()));
        }
    }
}
