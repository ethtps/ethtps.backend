using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Services.Attributes;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum
{
    [Provider("Polygon Hermez")]
    [RunsEvery(CronConstants.EVERY_13_S)]
    public sealed class PolygonHermezBlockInfoProvider : BlockInfoProviderBase
    {

        public PolygonHermezBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Polygon Hermez")
        {
        }

        public override async Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            var response = await _httpClient.GetAsync($"{Endpoint}/{blockNumber}");
            if (response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<Batch>(await response.Content.ReadAsStringAsync());
                var txCount = res.forgedTransactions;
                return new Block()
                {
                    BlockNumber = blockNumber,
                    Date = res.timestamp,
                    Settled = true,
                    TransactionCount = txCount
                };
            }
            else
            {
                throw new HttpRequestException($"Couldn't get batches: HTTP Response code {response.StatusCode}");
            }
        }

        public override Task<Block> GetBlockInfoAsync(DateTime time)
        {
            throw new NotImplementedException();
        }

        public override async Task<Block> GetLatestBlockInfoAsync()
        {
            var response = await _httpClient.GetAsync($"{Endpoint}?order=DESC&limit=20");
            if (response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<BatchResponse>(await response.Content.ReadAsStringAsync());
                var latestBatch = res.batches.OrderByDescending(x => x.batchNum).First();
                return await GetBlockInfoAsync(latestBatch.batchNum);
            }
            else
            {
                throw new HttpRequestException($"Couldn't get batches: HTTP Response code {response.StatusCode}");
            }
        }


        public sealed class TransactionListResponse
        {
            public Transaction[] transactions { get; set; }
            public int pendingItems { get; set; }
        }

        public sealed class Transaction
        {
            public string id { get; set; }
            public int itemId { get; set; }
            public string type { get; set; }
            public int position { get; set; }
            public string fromAccountIndex { get; set; }
            public string fromHezEthereumAddress { get; set; }
            public string fromBJJ { get; set; }
            public string toAccountIndex { get; set; }
            public object toHezEthereumAddress { get; set; }
            public object toBJJ { get; set; }
            public string amount { get; set; }
            public int batchNum { get; set; }
            public object historicUSD { get; set; }
            public DateTime timestamp { get; set; }
            public L1info L1Info { get; set; }
            public object L2Info { get; set; }
            public Token token { get; set; }
            public string L1orL2 { get; set; }
        }

        public sealed class L1info
        {
            public int toForgeL1TransactionsNum { get; set; }
            public bool userOrigin { get; set; }
            public string depositAmount { get; set; }
            public bool amountSuccess { get; set; }
            public bool depositAmountSuccess { get; set; }
            public float historicDepositAmountUSD { get; set; }
            public int ethereumBlockNum { get; set; }
        }

        public sealed class Token
        {
            public int id { get; set; }
            public int itemId { get; set; }
            public int ethereumBlockNum { get; set; }
            public string ethereumAddress { get; set; }
            public string name { get; set; }
            public string symbol { get; set; }
            public int decimals { get; set; }
            public float USD { get; set; }
            public DateTime fiatUpdate { get; set; }
        }


        public sealed class BatchResponse
        {
            public Batch[] batches { get; set; }
            public int pendingItems { get; set; }
        }

        public sealed class Batch
        {
            public int itemId { get; set; }
            public int batchNum { get; set; }
            public string ethereumTxHash { get; set; }
            public int ethereumBlockNum { get; set; }
            public string ethereumBlockHash { get; set; }
            public DateTime timestamp { get; set; }
            public string forgerAddr { get; set; }
            public Collectedfees collectedFees { get; set; }
            public float historicTotalCollectedFeesUSD { get; set; }
            public string stateRoot { get; set; }
            public int numAccounts { get; set; }
            public string exitRoot { get; set; }
            public int forgeL1TransactionsNum { get; set; }
            public int slotNum { get; set; }
            public int forgedTransactions { get; set; }
        }

        public sealed class Collectedfees
        {
            public string _1 { get; set; }
        }

    }
}
