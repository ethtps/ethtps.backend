using System;
using System.Linq;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.Extensions.DateTimeExtensions;
using ETHTPS.Data.Core.Models.DataEntries;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum
{
    [Provider("ZKSync Era")]
    [RunsEvery(CronConstants.EVERY_30_S)]
    public sealed class ZKSsyncEraBlockInfoProvider : BlockInfoProviderBase
    {
        private readonly string _latestBlockURL;
        private readonly string _blockURL;
        public ZKSsyncEraBlockInfoProvider(DBConfigurationProviderWithCache configurationProvider) : base(configurationProvider, "ZKSync Era")
        {
            _latestBlockURL = _configurationStrings.First(x => x.Name == "LatestBlockURL").Value;
            _blockURL = _configurationStrings.First(x => x.Name == "BlockURL").Value;

            if (string.IsNullOrWhiteSpace(_latestBlockURL))
            {
                throw new ArgumentNullException(nameof(_latestBlockURL));
            }
            if (string.IsNullOrWhiteSpace(_blockURL))
            {
                throw new ArgumentNullException(nameof(_blockURL));
            }
        }

        public override async Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            var obj = JsonConvert.DeserializeObject<dynamic>(await _httpClient.GetStringAsync(new Uri($"{Endpoint}/{_blockURL}/{blockNumber}", UriKind.Absolute)));
            var block = obj;
            return new Block()
            {
                Date = DateTimeExtensions.FromUnixTime(long.Parse(block.timestamp.ToString())),
                BlockNumber = int.Parse(block.number.ToString()),
                TransactionCount = int.Parse(block.l2TxCount.ToString()) + int.Parse(block.l1TxCount.ToString())
            };
        }

        public override Task<Block> GetBlockInfoAsync(DateTime time)
        {
            throw new NotImplementedException();
        }

        public override async Task<Block> GetLatestBlockInfoAsync()
        {
            var blocks = JsonConvert.DeserializeObject<dynamic>(await _httpClient.GetStringAsync(new Uri
                ($"{Endpoint}/{_latestBlockURL}", UriKind.Absolute)));
            return await GetBlockInfoAsync(int.Parse(blocks[0].number.ToString()));
        }
    }
}
