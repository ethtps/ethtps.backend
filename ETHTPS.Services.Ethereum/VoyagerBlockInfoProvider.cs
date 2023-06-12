using System;
using System.Linq;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.Extensions.DateTimeExtensions;
using ETHTPS.Data.Core.Models.DataEntries;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum
{
    [Provider("Starknet")]
    [RunsEvery(CronConstants.EVERY_13_S)]
    public sealed class VoyagerBlockInfoProvider : BlockInfoProviderBase
    {

        public VoyagerBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Starknet")
        {
        }

        public override Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            throw new NotImplementedException();
        }

        public override Task<Block> GetBlockInfoAsync(DateTime time)
        {
            throw new NotImplementedException();
        }

        public override async Task<Block> GetLatestBlockInfoAsync()
        {
            var responseString = await _httpClient.GetStringAsync("https://voyager.online/api/blocks?ps=10&p=1");
            var response = JsonConvert.DeserializeObject<StarknetResponse>(responseString);
            var settledBlocks = response.items.Where(x => x.status == "Accepted on L1").OrderByDescending(x => x.timestamp);
            var first = settledBlocks.ElementAt(0);
            var second = settledBlocks.ElementAt(1);
            BlockTimeSeconds = DateTimeExtensions.FromUnixTime(first.timestamp).Subtract(DateTimeExtensions.FromUnixTime(second.timestamp)).TotalSeconds;
            return new Block()
            {
                Date = DateTimeExtensions.FromUnixTime(first.timestamp),
                TransactionCount = first.txnCount
            };
        }


        public sealed class StarknetResponse
        {
            public StarknetBlockInfo[] items { get; set; }
            public int lastPage { get; set; }
        }

        public sealed class StarknetBlockInfo
        {
            public string id { get; set; }
            public string previousBlockId { get; set; }
            public int sequence { get; set; }
            public string stateRoot { get; set; }
            public int timestamp { get; set; }
            public int txnCount { get; set; }
            public string status { get; set; }
            public string l1VerificationTxHash { get; set; }
        }

    }
}
