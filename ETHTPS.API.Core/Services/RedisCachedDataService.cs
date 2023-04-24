using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.Data.Core;

using Newtonsoft.Json;

using StackExchange.Redis;

namespace ETHTPS.API.Core.Services
{
    public sealed class RedisCachedDataService : ICachedDataService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;


        public RedisCachedDataService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;

            _database = _connectionMultiplexer.GetDatabase();
        }

        public async Task<T?> GetDataAsync<T>(string key)
        {
            if (await HasDataAsync(key))
            {
                var value = await _database.StringGetAsync(key);
                if (!value.IsNull)
                {
                    return JsonConvert.DeserializeObject<T>(value.ToString());
                }
            }
            return default(T);
        }

        public async Task<bool> HasDataAsync(string key) => await _database.KeyExistsAsync(key);

        public async Task<bool> SetDataAsync(string key, string value) => await _database.StringSetAsync(key, value);

        public async Task<bool> SetDataAsync<T>(string key, T value) => await SetDataAsync(key, JsonConvert.SerializeObject(value));

        public async Task<bool> SetDataAsync<T>(T value) where T : ICachedKey => await SetDataAsync(value.ToCacheKey(), JsonConvert.SerializeObject(value));
    }
}
