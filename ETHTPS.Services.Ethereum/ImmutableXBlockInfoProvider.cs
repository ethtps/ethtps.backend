﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.Extensions.DateTimeExtensions;
using ETHTPS.Data.Core.Models.DataEntries;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum
{
    /// <summary>
    /// Immutable X doesn't have blocks but we can work around this by getting all transactions made in the past minute and assuming they are part of a block.
    /// </summary>
    [Provider("Immutable X")]
    [RunsEvery(CronConstants.EVERY_13_S)]
    public sealed class ImmutableXBlockInfoProvider : BlockInfoProviderBase
    {
        public ImmutableXBlockInfoProvider(DBConfigurationProviderWithCache configuration) : base(configuration, "Immutable X")
        {
            BlockTimeSeconds = 60;
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
            var txLimit = 200;
            var block = await GenerateFakeBlockAsync(txLimit);
            if (block == null)
            {
                return null;
            }
            while (block.TransactionCount == txLimit) //Increase the limit until we get all transactions
            {
                txLimit *= 2; //An exponential increase will reduce the number of calls
                block = await GenerateFakeBlockAsync(txLimit);
            }
            return block;
        }

        private async Task<Block> GenerateFakeBlockAsync(int txLimit)
        {
            var payload = new GraphQLPayload()
            {
                operationName = "listTransactions",
                variables = new Variables()
                {
                    address = "global"
                },
                query = "query listTransactions($address: String!, $nextToken: String) {\n  listTransactions(address: $address, limit: " + txLimit + ", nextToken: $nextToken) {\n    items {\n      txn_time\n      txn_id\n      txn_type\n      transfers {\n        from_address\n        to_address\n        token {\n          type\n          quantity\n          usd_rate\n          token_address\n          token_id\n          __typename\n        }\n        __typename\n      }\n      __typename\n    }\n    nextToken\n    lastUpdated\n    __typename\n  }\n}\n"
            };
            var message = new HttpRequestMessage()
            {
                Content = JsonContent.Create(payload),
                Method = HttpMethod.Post
            };
            var response = await _httpClient.SendAsync(message);
            if (response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<Rootobject>(await response.Content.ReadAsStringAsync());
                var lastMinuteItems = obj.data.listTransactions.items.Where(x => DateTimeExtensions.FromUnixTime(long.Parse(x.txn_time.Substring(0, x.txn_time.Length - 3))).ToUniversalTime() >= DateTime.Now.ToUniversalTime().Subtract(TimeSpan.FromMinutes(1)));
                return new Block()
                {
                    TransactionCount = lastMinuteItems.Count(),
                    Date = DateTime.Now.Subtract(TimeSpan.FromSeconds(DateTime.Now.Second)).Subtract(TimeSpan.FromMilliseconds(DateTime.Now.Millisecond)),
                    GasUsed = CalculateGasCost(lastMinuteItems)
                };
            }
            return null;
        }

        private int CalculateGasCost(IEnumerable<Item> items)
        {
            int sum = 0;
            foreach (var item in items)
            {
                switch (item.txn_type)
                {
                    case "mint":
                        sum += 150000;
                        break;
                    case "transfer":
                        sum += 25000;
                        break;
                    case "withdrawal":
                        sum += 21000;
                        break;
                    case "trade":
                        sum += 46000;
                        break;
                    case "deposit":
                        sum += 21000;
                        break;
                }
            }
            return sum;
        }

        public sealed class Rootobject
        {
            public Data data { get; set; }
        }

        public sealed class Data
        {
            public Listtransactions listTransactions { get; set; }
        }

        public sealed class Listtransactions
        {
            public Item[] items { get; set; }
            public string nextToken { get; set; }
            public object lastUpdated { get; set; }
            public string __typename { get; set; }
        }

        public sealed class Item
        {
            public string txn_time { get; set; }
            public int txn_id { get; set; }
            public string txn_type { get; set; }
            public Transfer[] transfers { get; set; }
            public string __typename { get; set; }
        }

        public sealed class Transfer
        {
            public string from_address { get; set; }
            public string to_address { get; set; }
            public Token token { get; set; }
            public string __typename { get; set; }
        }

        public sealed class Token
        {
            public string type { get; set; }
            public float quantity { get; set; }
            public float? usd_rate { get; set; }
            public string token_address { get; set; }
            public string token_id { get; set; }
            public string __typename { get; set; }
        }


        public sealed class GraphQLPayload
        {
            public string operationName { get; set; }
            public Variables variables { get; set; }
            public string query { get; set; }
        }

        public sealed class Variables
        {
            public string address { get; set; }
        }

    }
}
