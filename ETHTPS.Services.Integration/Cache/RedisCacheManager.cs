using Newtonsoft.Json;
using StackExchange.Redis;

namespace ETHTPS.Services.Integration.Cache
{
    public class RedisCacheManager<T> : ICacheManager<T>
    {
        private readonly IDatabase _database;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public RedisCacheManager(string connectionString)
        {
            var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            _database = connectionMultiplexer.GetDatabase();
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
        }

        public void Set(string key, T value)
        {
            string serializedValue = JsonConvert.SerializeObject(value, _jsonSerializerSettings);
            _database.StringSet(key, serializedValue);
        }

        public T? Get(string key)
        {
            string? serializedValue = _database.StringGet(key);
            if (serializedValue == null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(serializedValue, _jsonSerializerSettings);
        }

        public void Remove(string key)
        {
            _database.KeyDelete(key);
        }
    }
}
