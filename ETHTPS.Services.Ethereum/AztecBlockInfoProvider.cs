using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.Models.DataEntries;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum
{
    [Provider("Aztec")]
    [RunsEvery(CronConstants.EVERY_13_S)]
    public sealed class AztecBlockInfoProvider : BlockInfoProviderBase
    {

        public AztecBlockInfoProvider(DBConfigurationProviderWithCache provider) : base(provider, "Aztec")
        {

        }

        public override async Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            var payload = new GraphQLPayload()
            {
                operationName = "Block",
                variables = new IDObject()
                {
                    id = blockNumber
                },
                query = "query Block($id: Int!) {\n block: rollup(id: $id) {\n id\n hash\n ethTxHash\n proofData\n dataRoot\n nullifierRoot\n txs {\n id\n proofId\n __typename\n }\n created\n mined\n __typename\n }\n}\n"
            };
            var message = new HttpRequestMessage()
            {
                Content = JsonContent.Create(payload),
                Method = HttpMethod.Post
            };
            var response = await _httpClient.SendAsync(message);
            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<dynamic>(str);
                try
                {
                    return new Block()
                    {
                        BlockNumber = blockNumber,
                        TransactionCount = obj.data.block.txs.Count,
                        Date = DateTime.Parse(obj.data.block.created.ToString()).ToLocalTime(),
                        Settled = obj.data.block.mined != null
                    };
                }
                catch
                {

                }
            }
            return null;
        }

        public override Task<Block> GetBlockInfoAsync(DateTime time)
        {
            throw new NotImplementedException();
        }

        public override async Task<Block> GetLatestBlockInfoAsync()
        {
            var payload = new GraphQLPayload()
            {
                operationName = null,
                variables = new object(),
                query = "{\n  totalBlocks: totalRollups\n  totalTxs\n  serverStatus {\n    nextPublishTime\n    pendingTxCount\n    __typename\n  }\n}\n"
            };
            var message = new HttpRequestMessage()
            {
                Content = JsonContent.Create(payload),
                Method = HttpMethod.Post
            };
            var response = await _httpClient.SendAsync(message);
            if (response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<BlockCountRootObject>(await response.Content.ReadAsStringAsync());
                var block = await GetBlockInfoAsync(obj.data.totalBlocks);
                while (block == null || !block.Settled)
                {
                    block = await GetBlockInfoAsync(--obj.data.totalBlocks);
                }
                return block;
            }
            return null;
        }




        private class IDObject
        {
            public int id { get; set; }
        }

        private class GraphQLPayload
        {
            public string operationName { get; set; }
            public object variables { get; set; }
            public string query { get; set; }
        }

        public sealed class BlockCountRootObject
        {
            public BlockCountResponse data { get; set; }
        }

        public sealed class BlockCountResponse
        {
            public int totalBlocks { get; set; }
            public int totalTxs { get; set; }
            public Serverstatus serverStatus { get; set; }
        }

        public sealed class Serverstatus
        {
            public DateTime nextPublishTime { get; set; }
            public int pendingTxCount { get; set; }
            public string __typename { get; set; }
        }

    }
}
