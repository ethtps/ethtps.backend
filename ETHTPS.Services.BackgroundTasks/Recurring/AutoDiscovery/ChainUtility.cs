// (c) https://github.com/DefiLlama/chainlist/blob/main/utils/fetch.js
// DefiLlama's chainlist code fed to and regurgitated by ChatGPT

namespace ETHTPS.Services.BackgroundTasks.Recurring.AutoDiscovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public class ChainUtility
    {
        public class Chain
        {
            public required string Name { get; set; }
            public required string ChainId { get; set; }
            public required string ChainSlug { get; set; }
            public List<string> Rpc { get; set; } = new();
            public string? Icon { get; set; }
            public List<string>? Faucets { get; set; }
            public NativeCurrency? NativeCurrency { get; set; }
            public string? InfoURL { get; set; }
            public string? ShortName { get; set; }
            public UInt64? NetworkId { get; set; }
            public UInt64? Slip44 { get; set; }
            public Ens? Ens { get; set; }
            public List<Explorer>? Explorers { get; set; }
            public double? Tvl { get; internal set; }
        }

        public class NativeCurrency
        {
            public string? Name { get; set; }
            public string? Symbol { get; set; }
            public int? Decimals { get; set; }
        }

        public class Ens
        {
            public string? Registry { get; set; }
        }

        public class Explorer
        {
            public string? Name { get; set; }
            public string? Url { get; set; }
            public string? Standard { get; set; }
        }


        public class ChainTvl
        {
            public required string GeckoId { get; set; }
            public required double Tvl { get; set; }
            public string? TokenSymbol { get; set; }
            public string? CmcId { get; set; }
            public required string Name { get; set; }
            public required UInt64? ChainId { get; set; }
        }


        public class RpcObject
        {
            public required string Url { get; set; }
        }

        private static async Task<string> Fetcher(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(url);
                return response;
            }
        }

        private static string RemoveEndingSlash(string rpc)
        {
            return rpc.EndsWith("/") ? rpc.Substring(0, rpc.Length - 1) : rpc;
        }

        private static RpcObject? RemoveEndingSlashObject(object rpc)
        {
            if (rpc is string rpcString)
            {
                return new RpcObject { Url = RemoveEndingSlash(rpcString) };
            }
            else if (rpc is RpcObject rpcObj)
            {
                rpcObj.Url = RemoveEndingSlash(rpcObj?.Url ?? "");
                return rpcObj;
            }
            throw new ArgumentException("Invalid argument type", nameof(rpc));
        }

        private static Chain PopulateChain(Chain chain, List<ChainTvl> chainTvls, Dictionary<string, RpcObject[]> allExtraRpcs, Dictionary<string, string> chainIds)
        {
            if (allExtraRpcs.TryGetValue(chain.ChainId, out RpcObject[]? extraRpcs))
            {
                var rpcs = extraRpcs.Select(RemoveEndingSlashObject).ToList();

                foreach (var rpc in chain.Rpc)
                {
                    var rpcObj = RemoveEndingSlashObject(rpc);
                    if (!rpcs.Any(r => r?.Url == rpcObj?.Url))
                    {
                        rpcs.Add(rpcObj);
                    }
                }

                chain.Rpc = rpcs.Select(r => r?.Url ?? "").ToList();
            }
            else
            {
                chain.Rpc = chain.Rpc.Select(RemoveEndingSlash).ToList();
            }

            if (chainIds.TryGetValue(chain.ChainId, out string? chainSlug))
            {
                chain.ChainSlug = chainSlug;
                var defiChain = chainTvls.FirstOrDefault(c => c.Name.ToLower() == chainSlug);

                if (defiChain != null)
                {
                    chain.Tvl = defiChain.Tvl;
                }
            }

            return chain;
        }

        public static async Task<IEnumerable<Chain>> GenerateChainDataAsync() => await GenerateChainDataAsync(new Dictionary<string, RpcObject[]>(), new Dictionary<string, string>());

        public static async Task<IEnumerable<Chain>> GenerateChainDataAsync(Dictionary<string, RpcObject[]> allExtraRpcs, Dictionary<string, string> chainIds)
        {
            var chainsJson = await Fetcher("https://chainid.network/chains.json");
            var chainTvlsJson = await Fetcher("https://api.llama.fi/chains");

            var chains = JsonConvert.DeserializeObject<List<Chain>>(chainsJson);
            var chainTvls = JsonConvert.DeserializeObject<List<ChainTvl>>(chainTvlsJson);

            var sortedChains = chains?
                .Where(c => c.Name != "420coin")
                .Select(chain => PopulateChain(chain, chainTvls!, allExtraRpcs, chainIds))
                .OrderByDescending(c => c.Tvl ?? 0)
                .ToList();

            return sortedChains ?? Enumerable.Empty<Chain>();
        }
    }

}
