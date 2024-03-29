﻿namespace ETHTPS.Core
{
    /// <summary>
    /// Provides functionality for working with Redis Cache.
    /// </summary>
    public interface IRedisCacheService
    {
        public Task<bool> HasKeyAsync(string key);
        public bool HasKey(string key);
        public Task<T?> GetDataAsync<T>(string key);
        public T? GetData<T>(string key);
        public void UpdateData<T>(string key, Action<T> updateAction) where T : ICachedKey;
        public Task UpdateDataAsync<T>(string key, Action<T> updateAction) where T : ICachedKey;
        public Task<bool> SetDataAsync(string key, string value);
        public Task<bool> SetDataAsync<T>(string key, T value);
        public Task<bool> SetDataAsync<T>(T value) where T : ICachedKey;
        public Task<bool> SetDataAsync(string key, string value, TimeSpan expiration);
        public Task<bool> SetDataAsync<T>(string key, T value, TimeSpan expiration);
        public bool SetData<T>(string key, T value, TimeSpan expiration);
        public Task<bool> SetDataAsync<T>(T value, TimeSpan expiration) where T : ICachedKey;
        public Task<TimeSpan?> GetTimeToLiveAsync(string key);
        public Task<bool> ExpireAsync(string key, TimeSpan expiration);
    }
}
