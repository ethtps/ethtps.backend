using ETHTPS.Data.Core;

namespace ETHTPS.API.BIL.Infrastructure.Services.DataServices
{
    public interface ICachedDataService
    {
        public Task<bool> HasDataAsync(string key);
        public Task<bool> SetDataAsync(string key, string value);
        public Task<bool> SetDataAsync<T>(string key, T value);
        public Task<T?> GetDataAsync<T>(string key);
        public Task<bool> SetDataAsync<T>(T value) where T : ICachedKey;
    }
}
