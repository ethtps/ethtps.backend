using System;
using System.Linq;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.Models.DataEntries;

using Fizzler.Systems.HtmlAgilityPack;

using HtmlAgilityPack;

namespace ETHTPS.Services.Ethereum
{
    [Provider("Ronin")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class RoninBlockInfoProvider : BlockInfoProviderBase
    {
        private readonly string _baseURL = string.Empty;

        public RoninBlockInfoProvider(DBConfigurationProviderWithCache configuration) : base(configuration, "Ronin")
        {
            BlockTimeSeconds = 3;
        }

        public override Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            HtmlWeb web = new HtmlWeb();
            //HtmlDocument doc = web.Load($"{_baseURL}/block/{blockNumber}");

            var txPage = web.Load($"{_baseURL}/block/{blockNumber}/txs");
            var txCountNode = txPage.DocumentNode.QuerySelectorAll(TXCountSelector);
            var txCount = new string(string.Join(" ", txCountNode.Select(x => x.InnerText)).Where(Char.IsNumber).ToArray());

            return Task.FromResult(new Block()
            {
                BlockNumber = blockNumber,
                TransactionCount = int.Parse(txCount)
            });
        }

        public override Task<Block> GetBlockInfoAsync(DateTime time)
        {
            throw new NotImplementedException();
        }

        public override async Task<Block> GetLatestBlockInfoAsync()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load($"{_baseURL}/blocks");

            var blockHeightNode = doc.DocumentNode.QuerySelectorAll(BlockHeightSelector);
            var blockHeight = string.Join(" ", blockHeightNode.Select(x => x.InnerText));

            if (string.IsNullOrWhiteSpace(blockHeight))
            {
                throw new Exception("Couldn't get block height");
            }
            var result = await GetBlockInfoAsync(int.Parse(blockHeight) - 1);
            result.Date = DateTime.Now;
            return result;
        }


        public sealed class TxSummaryResponse
        {
            public TxSummaryPageprops pageProps { get; set; }
            public bool __N_SSG { get; set; }
        }

        public sealed class TxSummaryPageprops
        {
            public Overviewdata overviewData { get; set; }
            public Txchartdata txChartData { get; set; }
        }

        public sealed class Overviewdata
        {
            public int blockTime { get; set; }
            public int totalAddresses { get; set; }
            public int totalBlocks { get; set; }
            public int totalTxs { get; set; }
        }

        public sealed class Txchartdata
        {
            public int[] txCount { get; set; }
            public string[] label { get; set; }
        }

    }
}
