﻿using System;
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
    [Provider("Nahmii 2.0")]
    [RunsEvery(CronConstants.EVERY_13_S)]
    public sealed class Nahmii20BlockInfoProvider : BlockInfoProviderBase
    {

        public Nahmii20BlockInfoProvider(DBConfigurationProviderWithCache configurationProvider) : base(configurationProvider, "Nahmii 2.0")
        {

        }

        public override Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load($"https://explorer.nahmii.io/blocks/{blockNumber}/transactions");

            var txCountNode = doc.DocumentNode.QuerySelectorAll("span.mr-4:nth-child(1)");
            var txCount = new string(string.Join(" ", txCountNode.Select(x => x.InnerText)).Where(Char.IsNumber).ToArray());

            var dateNode = doc.DocumentNode.QuerySelectorAll("span.mr-4:nth-child(3)");
            var date = string.Join(" ", dateNode.Select(x => x.Attributes["data-from-now"].Value.Replace(".000000Z", "")));

            var gasNode = doc.DocumentNode.QuerySelectorAll("div.card:nth-child(2) > div:nth-child(1) > div:nth-child(2) > h3:nth-child(1)");
            var gas = new string(string.Join(" ", gasNode.Select(x => x.InnerText.UntilParanthesis())).Where(Char.IsNumber).ToArray());

            return Task.FromResult(new Block()
            {
                TransactionCount = ValueOrZero(txCount),
                Date = DateTime.Parse(date),
                BlockNumber = blockNumber,
                GasUsed = ValueOrZero(gas)
            });
        }

        private int ValueOrZero(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }
            else return int.Parse(value);
        }

        public override Task<Block> GetBlockInfoAsync(DateTime time)
        {
            throw new NotImplementedException();
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

        public override async Task<Block> GetLatestBlockInfoAsync()
        {
            var responseString = await _httpClient.GetStringAsync("https://explorer.nahmii.io/blocks?type=JSON");
            var response = JsonConvert.DeserializeObject<dynamic>(responseString);
            string firstBlockData = response.items[0].ToString();
            var firstBlockNumber = GetBlockNumberFromHTML(firstBlockData);
            return await GetBlockInfoAsync(firstBlockNumber);
        }
    }
}
