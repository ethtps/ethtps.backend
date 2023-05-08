using System;
using System.Linq;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Services.Attributes;
using ETHTPS.Services.Ethereum.JSONRPC;

using Fizzler.Systems.HtmlAgilityPack;

using HtmlAgilityPack;

namespace ETHTPS.Services.Ethereum
{
    [Provider("Arbitrum Nova")]
    [RunsEvery(CronConstants.EVERY_10_S)]
    public sealed class ArbitrumNovaBlockInfoProvider : JSONRPCBlockInfoProviderBase
    {
        private const string NAME = "Arbitrum Nova";

        public ArbitrumNovaBlockInfoProvider(IDBConfigurationProvider configuration) : base(configuration, NAME)
        {

        }

        //Arbitrum Nova doesn't have an implementation for eth_getBlockByNumber yet
        public override Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load($"https://nova-explorer.arbitrum.io/block/{blockNumber}/transactions");
            var txCountNode = doc.DocumentNode.QuerySelectorAll(TXCountSelector);
            var txCount = new string(string.Join(" ", txCountNode.Select(x => x.InnerText)).Where(Char.IsNumber).ToArray());

            var gasNode = doc.DocumentNode.QuerySelectorAll(GasUsedSelector);
            var gas = new string(string.Join(" ", gasNode.Select(x => x.InnerText.Substring(0, x.InnerText.IndexOf("|")))).Where(Char.IsNumber).ToArray());

            var dateNode = doc.DocumentNode.QuerySelectorAll(DateSelector);
            var date = string.Join(" ", dateNode.Select(x => x.Attributes["data-from-now"].Value));
            return Task.FromResult(new Block()
            {
                TransactionCount = int.Parse(txCount),
                Date = DateTime.Parse(date),
                BlockNumber = blockNumber,
                GasUsed = double.Parse(gas)
            });
        }
    }
}
