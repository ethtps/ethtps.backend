using ETHTPS.Data.Core;

namespace ETHTPS.API.BIL.Infrastructure.Services.DataServices
{
    public interface IRedisCacheService
    {
        public Task<bool> HasKeyAsync(string key);
        public Task<T?> GetDataAsync<T>(string key);
        public Task<bool> SetDataAsync(string key, string value);
        public Task<bool> SetDataAsync<T>(string key, T value);
        public Task<bool> SetDataAsync<T>(T value) where T : ICachedKey;
        public Task<bool> SetDataAsync(string key, string value, TimeSpan expiration);
        public Task<bool> SetDataAsync<T>(string key, T value, TimeSpan expiration);
        public Task<bool> SetDataAsync<T>(T value, TimeSpan expiration) where T : ICachedKey;
        public Task<TimeSpan?> GetTimeToLiveAsync(string key);
        public Task<bool> ExpireAsync(string key, TimeSpan expiration);
    }
}
