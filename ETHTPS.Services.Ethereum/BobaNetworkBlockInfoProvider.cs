using System;
using System.Linq;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.Extensions.StringExtensions;
using ETHTPS.Data.Core.Models.DataEntries;

using Fizzler.Systems.HtmlAgilityPack;

using HtmlAgilityPack;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum
{
    [Provider("Boba Network")]
    [RunsEvery(CronConstants.EVERY_13_S)]
    public sealed class BobaNetworkBlockInfoProvider : BlockInfoProviderBase
    {
        private const string _NAME = "Boba Network";
        public BobaNetworkBlockInfoProvider(DBConfigurationProviderWithCache configuration) : base(configuration, _NAME)
        {

        }
        private static int GetBlockNumberFromHTML(string html)
        {
            var str = "data-block-number";
            var index = html.IndexOfOccurence(str, 1);
            var str2 = "data-block-hash";
            var lineBreakIndex = html.IndexOf(str2);
            var targetString = html.Substring(index + str.Length + 2, (lineBreakIndex - index) - 4 - str2.Length);
            return int.Parse(new string(targetString.Where(char.IsDigit).ToArray()));
        }

        public override Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load($"{Endpoint}/{blockNumber}/transactions");
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
                GasUsed = int.Parse(gas)
            });
        }

        public override Task<Block> GetBlockInfoAsync(DateTime time)
        {
            throw new NotImplementedException();
        }

        public override async Task<Block> GetLatestBlockInfoAsync()
        {
            var obj = JsonConvert.DeserializeObject<dynamic>(await _httpClient.GetStringAsync("https://blockexplorer.boba.network/blocks?type=JSON"));
            var latest = obj.items[0].ToString();
            return await GetBlockInfoAsync(GetBlockNumberFromHTML(latest));
        }
    }
}
