using System;

using ETHTPS.Data.Core.Extensions;

namespace ETHTPS.Services.Ethereum.Scan
{
    public class ScanRequestModelFactory
    {
        private readonly string _apiKey;

        public ScanRequestModelFactory(string apiKey)
        {
            _apiKey = apiKey;
        }

        public GetBlockNumberByTimestampRequestModel CreateGetBlockNumberByTimestampRequest(DateTime time) => new(_apiKey, time.ToUnixTime());
    }
}
