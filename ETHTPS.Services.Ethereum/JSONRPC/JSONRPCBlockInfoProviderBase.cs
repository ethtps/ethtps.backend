using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Extensions;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Services.Ethereum.JSONRPC.Models;
using ETHTPS.Services.Ethereum.JSONRPC.Models.Exceptions;
using ETHTPS.Services.Infrastructure.Serialization;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum.JSONRPC
{
    public abstract class JSONRPCBlockInfoProviderBase : BlockInfoProviderBase
    {
        private readonly HttpClient _httpClient;
        private static DateTime _LastCallTime = DateTime.Now;
        private static TimeSpan _TIME_BETWEEN = TimeSpan.FromMilliseconds(500);
        private static bool _canCall => _timeSinceLastCall > _TIME_BETWEEN;
        private static bool _busy { get; set; }
        private static TimeSpan _timeSinceLastCall => DateTime.Now - _LastCallTime;

        public JSONRPCBlockInfoProviderBase(IDBConfigurationProvider configurationProvider, string providerName) : base(configurationProvider, providerName)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(Endpoint)
            };
            var authenticationString = $"{ProjectID}:{Secret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(authenticationString));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
        }

        public override double BlockTimeSeconds { get; set; }

        public override async Task<Block> GetBlockInfoAsync(int blockNumber)
        {
            try
            {
                while (_busy && !_canCall) await Task.Delay(_TIME_BETWEEN);
                _busy = true;
                var requestModel = JSONRPCRequestFactory.CreateGetBlockByBlockNumberRequest("0x" + blockNumber.ToString("X"));
                var json = requestModel.SerializeAsJsonWithEmptyArray();
                var message = new HttpRequestMessage()
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post
                };
                var response = await _httpClient.SendAsync(message);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<JSONRPCGetBlockByNumberResponseModel>(responseString);
                    if (responseObject == null || responseObject.result == null)
                        return null;
                    Block result = new()
                    {
                        BlockNumber = blockNumber,
                        Date = DateTimeExtensions.FromUnixTime(Convert.ToInt64(responseObject.result.timestamp, 16)),
                        TransactionCount = responseObject.result.transactions.Length,
                        Settled = true,
                        GasUsed = Convert.ToInt64(responseObject.result.gasUsed, 16)
                    };
                    return result;
                }
            }
            finally
            {
                _busy = false;
                _LastCallTime = DateTime.Now;
            }
            throw new JSONRPCRequestException(_httpClient.BaseAddress.ToString());
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
                Method = HttpMethod.Post
            };
            var response = await _httpClient.SendAsync(message);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<JSONRPCResponseModel>(responseString);
                var blockNumber = Convert.ToInt32(responseObject.Result, 16);
                return await GetBlockInfoAsync(blockNumber);
            }
            throw new JSONRPCRequestException(_httpClient.BaseAddress.ToString());
        }
    }
}
