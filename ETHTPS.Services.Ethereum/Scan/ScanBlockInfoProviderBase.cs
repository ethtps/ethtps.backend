using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Extensions.StringExtensions;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Services.Ethereum.Scan.Extensions;

using Fizzler.Systems.HtmlAgilityPack;

using HtmlAgilityPack;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum.Scan
{
    public abstract class ScanBlockInfoProviderBase : BlockInfoProviderBase
    {
        protected readonly HttpClient _httpClient;
        private readonly ScanRequestModelFactory _requestModelFactory;
        protected ScanBlockInfoProviderBase(IDBConfigurationProvider configuration, string providerName) : base(configuration, providerName)
        {
            _requestModelFactory = new ScanRequestModelFactory(APIKey);
        }

        public override Task<Block> GetBlockInfoAsync(int blockNumber)
        {

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Endpoint + blockNumber);
            if (_providerName == "Optimistic Ethereum") {; }
            var txCountNode = doc.DocumentNode.QuerySelectorAll(TXCountSelector);
            var txCount = new string(txCountNode.First().InnerText.RemoveAllNonNumericCharacters());

            string gasUsed = "0";
            if (!string.IsNullOrWhiteSpace(GasUsedSelector))
            {
                var gasUsedNode = doc.DocumentNode.QuerySelectorAll(GasUsedSelector);
                gasUsed = new string(gasUsedNode.First().InnerText.UntilParanthesis().RemoveAllNonNumericCharacters());
            }

            var dateNode = doc.DocumentNode.QuerySelectorAll(DateSelector);
            var dateString = dateNode.First().InnerText.BetweenParantheses().Replace(" +UTC", "");
            DateTime date;
            //(Sep-17-2021 06:40:08 AM +UTC) 
            if (DateTime.TryParseExact(dateString, "MMM-dd-yyyy hh:mm:ss tt", null, System.Globalization.DateTimeStyles.None, out date))
            {

            }

            var transactionCount = int.Parse(txCount);
            if (_providerName == "Arbiscan")
            {
                // There is a StartBlock transaction in every Arbitrum Block that should not count toward TPS
                transactionCount -= 1;
            }

            return Task.FromResult(new Block()
            {
                BlockNumber = blockNumber,
                GasUsed = double.Parse(gasUsed),
                TransactionCount = transactionCount,
                Date = date
            });
        }

        public override async Task<Block> GetBlockInfoAsync(DateTime time)
        {
            var requestModel = _requestModelFactory.CreateGetBlockNumberByTimestampRequest(time);
            var requestString = requestModel.ToQueryString();
            var blockNumberRequest = await _httpClient.GetAsync(requestString);
            string blockNumberString = JsonConvert.DeserializeObject<dynamic>(await blockNumberRequest.Content.ReadAsStringAsync()).result.ToString();
            int blockNumber;
            if (!int.TryParse(blockNumberString, out blockNumber))
            {
                throw new ArgumentException($"Error parsing block number\r\nDetails: \"{blockNumberString}\"");
            }
            return await GetBlockInfoAsync(blockNumber);
        }

        public override async Task<Block> GetLatestBlockInfoAsync() => await GetBlockInfoAsync(DateTime.Now.ToUniversalTime());
    }
}
