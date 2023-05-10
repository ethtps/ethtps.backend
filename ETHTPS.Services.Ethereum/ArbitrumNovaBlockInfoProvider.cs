using System;
using System.Linq;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Extensions;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Services.Attributes;

using Fizzler.Systems.HtmlAgilityPack;

using HtmlAgilityPack;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum
{
    [Provider("Arbitrum Nova")]
    [RunsEvery(CronConstants.EVERY_10_S)]
    public sealed class ArbitrumNovaBlockInfoProvider : BlockInfoProviderBase
    {
        private const string NAME = "Arbitrum Nova";

        public ArbitrumNovaBlockInfoProvider(IDBConfigurationProvider configuration) : base(configuration, NAME)
        {

        }

        //Arbitrum Nova doesn't have an implementation for eth_getBlockByNumber yet
        public override Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load($"{_configurationStrings.First(x => x.Name == "ExplorerEndpoint").Value}/block/{blockNumber}/transactions");
            var txCountNode = doc.DocumentNode.QuerySelectorAll(TXCountSelector);
            var txCount = new string(string.Join(" ", txCountNode.Select(x => x.InnerText)).Where(Char.IsNumber).ToArray());

            var gasNode = doc.DocumentNode.QuerySelectorAll(GasUsedSelector);
            var gas = new string(
                string.Join(" ",
                gasNode.Select(x =>
                x.InnerText.Substring(0, x.InnerText.IndexOf("("))))
                .Where(Char.IsNumber)
                .ToArray());

            var dateNode = doc.DocumentNode.QuerySelectorAll(DateSelector);
            var dateString = string.Join(" ", dateNode.Select(x => x.InnerHtml));
            //dateString = dateString.BetweenParantheses();
            return Task.FromResult(new Block()
            {
                TransactionCount = int.Parse(txCount),
                Date = DateTime.Now,//DateTime.Parse(dateString),
                BlockNumber = blockNumber,
                GasUsed = int.Parse(string.IsNullOrWhiteSpace(gas) ? "0" : gas)
            });
        }

        public override async Task<Block> GetBlockInfoAsync(DateTime time)
        {
            var responseString = await _httpClient.GetStringAsync($"{Endpoint}/api?module=block&action=getblocknobytime&timestamp={time.ToUnixTime()}&closest=before&apikey={APIKey}");
            var response = JsonConvert.DeserializeObject<GetBlockNumberByTimeResponseModel>(responseString);
            if (response.status == "1")
            {
                var result = await GetBlockInfoAsync(int.Parse(response.result));
                result.Date = time;
                return result;
            }
            throw new ApplicationException($"Couldn't get block at {time}. Response: ---> {responseString} <---");
        }

        public override Task<Block> GetLatestBlockInfoAsync() => GetBlockInfoAsync(DateTime.Now);


        public class GetBlockNumberByTimeResponseModel
        {
            public string status { get; set; }
            public string message { get; set; }
            public string result { get; set; }
        }

    }
}
