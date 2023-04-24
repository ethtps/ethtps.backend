using System;
using System.Linq;

using Fizzler.Systems.HtmlAgilityPack;

using HtmlAgilityPack;

namespace ETHTPS.Services.BlockchainServices.BlockTime
{
    public sealed class EthereumBlockTimeProvider : IBlockTimeProvider
    {
        private double _lastBlocktime = 13.5;

        public double GetBlockTime()
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                var page = web.Load($"https://ycharts.com/indicators/ethereum_average_block_time");
                var blockTimeNode = page.DocumentNode.QuerySelectorAll(".key-stat-title");
                var blockTime = double.Parse(new string(string.Join(" ", blockTimeNode.Select(x => x.InnerText)).Where(x => Char.IsNumber(x) || x == '.').ToArray()));
                _lastBlocktime = blockTime;
            }
            catch { } //Whatever

            return _lastBlocktime;
        }
    }
}
