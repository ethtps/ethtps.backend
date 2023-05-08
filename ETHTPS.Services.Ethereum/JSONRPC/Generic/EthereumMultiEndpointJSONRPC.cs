
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Services.Attributes;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ETHTPS.Services.Ethereum.JSONRPC.Generic
{
    [Provider("Ethereum")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class EthereumMultiEndpointJSONRPC : IHTTPBlockInfoProvider
    {
        private readonly IEnumerable<(IHTTPBlockInfoProvider Provider, int FailureCount)> _children;
        private static Random _random = new Random();
        private const int _BLACKLIST_AFTER = 5;
        private readonly string[] _endpoints;
        private readonly int _totalChildren;
        private int _currentChildIndex = 0;
        private readonly ILogger<EthereumMultiEndpointJSONRPC> _logger;
        public double BlockTimeSeconds { get; set; }

        public EthereumMultiEndpointJSONRPC(IConfiguration configuration, ILogger<EthereumMultiEndpointJSONRPC> logger)
        {
            _endpoints = configuration.GetSection("MultiEndpointJSONRPC").GetSection("Ethereum").Get<string[]>();
            _children = _endpoints.Select(x => (Provider: (IHTTPBlockInfoProvider)(new EthereumGenericJSONRPCBlockInfoProvider(x)), FailureCount: 0));
            _totalChildren = _endpoints.Length;
            _logger = logger;
        }

        public Task<Block> GetLatestBlockInfoAsync()
        {
            Block result = default;
            for (int i = 0; i < _totalChildren; i++)
            {
                if (result != null) return Task.FromResult(result);
            }
            return Task.FromResult(result);
        }

        public async Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            Block result = default;
            int c = 0;
            do
            {
                var next = _children.ElementAt(_random.Next(_totalChildren));
                if (next.FailureCount >= _BLACKLIST_AFTER)
                {
                    _logger.LogInformation("Blacklist updated. New size: " + _children.Count(x => x.FailureCount >= _BLACKLIST_AFTER));
                    continue;
                }
                result = await next.Provider.GetBlockInfoAsync(blockNumber);
                if (result != null)
                {
                    next.FailureCount = 0;
                    return result;
                }
                else
                {
                    next.FailureCount++;
                    _logger.LogInformation($"Updater failure count: {next.FailureCount}");
                }
            }
            while (result == null && ++c < _totalChildren);
            return result;
        }
        public Task<Block> GetBlockInfoAsync(DateTime time)
        {
            Block result = default;
            for (int i = 0; i < _totalChildren; i++)
            {
                if (result != null) return Task.FromResult(result);
            }
            return Task.FromResult(result);
        }
    }
}
