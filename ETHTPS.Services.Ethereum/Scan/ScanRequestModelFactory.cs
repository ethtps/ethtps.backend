using System;

using ETHTPS.Data.Core.Extensions.DateTimeExtensions;

namespace ETHTPS.Services.Ethereum.Scan
{
    public sealed class ScanRequestModelFactory
    {
        private readonly string _apiKey;

        public ScanRequestModelFactory(string apiKey)
        {
            _apiKey = apiKey;
        }

        public GetBlockNumberByTimestampRequestModel CreateGetBlockNumberByTimestampRequest(DateTime time) => new(_apiKey, time.ToUnixTime());
    }
}
