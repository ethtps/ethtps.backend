﻿using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Services.Ethereum.JSONRPC.Models;
using ETHTPS.Services.Infrastructure.Serialization;

using Fizzler.Systems.HtmlAgilityPack;

using HtmlAgilityPack;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum
{
    [Provider("Metis")]
    [RunsEvery(CronConstants.EVERY_13_S)]
    public sealed class MetisBlockInfoProvider : BlockInfoProviderBase
    {

        public MetisBlockInfoProvider(DBConfigurationProviderWithCache configuration) : base(configuration, "Metis")
        {
        }

        public override async Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load($"{Endpoint}/block/{blockNumber}/transactions");

            var dateNode = doc.DocumentNode.QuerySelectorAll(DateSelector);
            var date = string.Join(" ", dateNode.Select(x => x.Attributes["data-from-now"].Value)).Replace(".000000Z", string.Empty);
            var dateTime = DateTime.Parse(date);
            ;

            var gasNode = doc.DocumentNode.QuerySelectorAll(GasUsedSelector);
            var gas = new string(string.Join(" ", gasNode.Select(x => x.InnerText.Substring(0, x.InnerText.IndexOf("|")))).Where(Char.IsNumber).ToArray());

            var txCount = int.Parse(JsonConvert.DeserializeObject<dynamic>(await _httpClient.GetStringAsync($"{Endpoint}/block/{blockNumber}/transactions?type=JSON")).items.Count.ToString());

            return new Block()
            {
                BlockNumber = blockNumber,
                Date = dateTime,
                GasUsed = int.Parse(gas),
                Settled = true,
                TransactionCount = txCount
            };
        }

        public override Task<Block> GetBlockInfoAsync(DateTime time)
        {
            throw new NotImplementedException();
        }

        public override async Task<Block> GetLatestBlockInfoAsync()
        {
            var requestModel = JSONRPCRequestFactory.CreateGetBlockHeightRequest();
            var json = requestModel.SerializeAsJsonWithEmptyArray();
            var message = new HttpRequestMessage()
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri("/api/eth-rpc", UriKind.Relative)
            };
            var response = await _httpClient.SendAsync(message);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<JSONRPCResponseModel>(responseString);
                return await GetBlockInfoAsync(Convert.ToInt32(responseObject.Result, 16));
            }
            return null;
        }
    }
}
