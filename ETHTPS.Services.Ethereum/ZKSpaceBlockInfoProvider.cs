using System;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.Extensions.DateTimeExtensions;
using ETHTPS.Data.Core.Models.DataEntries;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum
{
    [Provider("ZKSpace")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class ZKSpaceBlockInfoProvider : BlockInfoProviderBase
    {
        public ZKSpaceBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "ZKSpace")
        {

        }

        public override async Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            var obj = JsonConvert.DeserializeObject<dynamic>(await _httpClient.GetStringAsync($"https://api.zkswap.info/v3/block/{blockNumber}"));
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
            var blocks = JsonConvert.DeserializeObject<dynamic>(await _httpClient.GetStringAsync("https://api.zkswap.info/v3/blocks?start=0&limit=2"));
            return await GetBlockInfoAsync(int.Parse(blocks.data.data[0].number.ToString()));
        }
    }
}
