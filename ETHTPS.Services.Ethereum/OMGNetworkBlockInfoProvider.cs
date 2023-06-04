using System;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Data.Core.Attributes;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum
{
    [Provider("OMG Network")]
    [RunsEvery(CronConstants.EVERY_13_S)]
    public sealed class OMGNetworkBlockInfoProvider : BlockInfoProviderBase
    {

        public OMGNetworkBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "OMG Network")
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
            var response = await _httpClient.PostAsync(Endpoint, null);
            var responseObject = JsonConvert.DeserializeObject<OMGResponseObject>(await response.Content.ReadAsStringAsync());
            if (responseObject == null) return null;
            var latestBlock = responseObject.data[0];
            var secondToLatestBlock = responseObject.data[1];
            BlockTimeSeconds = latestBlock.inserted_at.Subtract(secondToLatestBlock.inserted_at).TotalSeconds;
            return new Block()
            {
                BlockNumber = latestBlock.blknum,
                Date = latestBlock.inserted_at,
                TransactionCount = latestBlock.tx_count
            };
        }


        public sealed class OMGResponseObject
        {
            public BlockData[] data { get; set; }
            public Data_Paging data_paging { get; set; }
            public string service_name { get; set; }
            public bool success { get; set; }
            public string version { get; set; }
        }

        public sealed class Data_Paging
        {
            public int limit { get; set; }
            public int page { get; set; }
        }

        public sealed class BlockData
        {
            public int blknum { get; set; }
            public int eth_height { get; set; }
            public string hash { get; set; }
            public DateTime inserted_at { get; set; }
            public int timestamp { get; set; }
            public int tx_count { get; set; }
            public DateTime updated_at { get; set; }
        }

    }
}
