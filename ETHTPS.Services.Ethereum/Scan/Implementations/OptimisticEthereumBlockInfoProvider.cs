using System;
using System.Linq;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.Models.DataEntries;

using Fizzler.Systems.HtmlAgilityPack;

using HtmlAgilityPack;

namespace ETHTPS.Services.Ethereum.Scan.Implementations
{
    [Provider("Optimism")]
    [Disabled]
    [Obsolete("Use JSONRPC.OptimismBlockInfoProvider instead", true)]
    public sealed class OptimisticEthereumBlockInfoProvider : ScanBlockInfoProviderBase
    {
        private readonly string _targetElementSelector = "";
        public OptimisticEthereumBlockInfoProvider(IDBConfigurationProvider configuration) : base(configuration, "Optimistic Ethereum")
        {

        }

        public override Task<Block> GetLatestBlockInfoAsync()
        {
            HtmlWeb web = new();
            HtmlDocument doc = web.Load(Endpoint);

            var nodes = doc.DocumentNode.QuerySelectorAll(_targetElementSelector);
            var x = new string(nodes.First().InnerText.Where(c => char.IsNumber(c) || c == '.').ToArray());
            var data = new Block()
            {
                Date = DateTime.Now,
                TransactionCount = (int)(5 * float.Parse(x))
            };
            return Task.FromResult(data);
        }
    }
}
