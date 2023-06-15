using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.Data.Core;

using Newtonsoft.Json;

using NLog;

using StackExchange.Redis;

namespace ETHTPS.API.Core.Services
{
    public sealed class RedisCachedDataService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;
        private readonly ILogger? _logger;

        public RedisCachedDataService(IConnectionMultiplexer connectionMultiplexer, ILogger? logger = null)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
            _logger = logger;
        }

        public async Task<T?> GetDataAsync<T>(string key)
        {
            if (await HasKeyAsync(key))
            {
                var value = await _database.StringGetAsync(key);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return JsonConvert.DeserializeObject<T>(value.ToString(), new JsonSerializerSettings()
                    {
                        Error = (sender, args) =>
                        {
                            _logger?.Error(args);
                            args.ErrorContext.Handled = true;
                        }
                    });
                }
            }
            return default(T);
        }

        public T? GetData<T>(string key)
        {
            if (HasKey(key))
            {
                var value = _database.StringGet(key);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return JsonConvert.DeserializeObject<T>(value.ToString(), new JsonSerializerSettings()
                    {
                        Error = (sender, args) =>
                        {
                            _logger?.Error(args);
                            args.ErrorContext.Handled = true;
                        }
                    });
                }
            }
            return default;
        }

        public async Task<bool> HasKeyAsync(string key) => await _database.KeyExistsAsync(key);
        public bool HasKey(string key) => _database.KeyExists(key);

        public async Task<bool> SetDataAsync(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key)) return false;
            return await _database.StringSetAsync(key, value);
        }

        public async Task<bool> SetDataAsync<T>(string key, T value)
        {
            if (value == null || string.IsNullOrWhiteSpace(key)) return false;
            return await SetDataAsync(key, JsonConvert.SerializeObject(value));
        }

        public async Task<bool> SetDataAsync<T>(T value) where T : ICachedKey
        {
            if (value == null) return false;
            return await SetDataAsync(value.ToCacheKey(), JsonConvert.SerializeObject(value));
        }

        public async Task<bool> SetDataAsync(string key, string value, TimeSpan expiration)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key) || expiration.TotalMilliseconds <= 0) return false;
            return await _database.StringSetAsync(key, value, expiration);
        }

        public async Task<bool> SetDataAsync<T>(string key, T value, TimeSpan expiration)
        {
            if (value == null || string.IsNullOrWhiteSpace(key) || expiration.TotalMilliseconds <= 0) return false;
            return await SetDataAsync(key, JsonConvert.SerializeObject(value), expiration);
        }

        public async Task<bool> SetDataAsync<T>(T value, TimeSpan expiration) where T : ICachedKey
        {
            if (value == null || expiration.TotalMilliseconds <= 0) return false;
            return await SetDataAsync(value.ToCacheKey(), JsonConvert.SerializeObject(value), expiration);
        }

        public async Task<TimeSpan?> GetTimeToLiveAsync(string key)
        {
            var timeSpan = await _database.KeyTimeToLiveAsync(key);
            return timeSpan;
        }

        public async Task<bool> ExpireAsync(string key, TimeSpan expiration)
        {
            return await _database.KeyExpireAsync(key, expiration);
        }

        public void UpdateData<T>(string key, Action<T> updateAction)
            where T : ICachedKey => Task.Run(async () => await UpdateDataAsync(key, updateAction));

        /// <summary>
        /// Updates a value asynchronously. If the key does not exist, nothing happens.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="updateAction">The update action.</param>
        public async Task UpdateDataAsync<T>(string key, Action<T> updateAction)
            where T : ICachedKey
        {
            if (!await HasKeyAsync(key)) return;

            var existing = await GetDataAsync<T>(key);
            if (existing == null)
            {
                await _database.KeyDeleteAsync(key);
                return;
            }
            updateAction(existing);
            await SetDataAsync<T>(key, existing);
        }
    }

}
