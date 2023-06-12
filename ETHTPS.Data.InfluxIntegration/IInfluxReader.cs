using ETHTPS.Data.Core;

using InfluxDB.Client;

namespace ETHTPS.Data.Integrations.InfluxIntegration
{
    /// <summary>
    /// Provides InfluxDB read operations
    /// </summary>
    public interface IInfluxReader
    {
        public Task<IEnumerable<string>> GetBucketsAsync();
        public Task<bool> BucketExistsAsync(string bucket);
        public IAsyncEnumerable<TMeasurement> GetEntriesBetween<TMeasurement>(string bucket, string measurement, DateTime start, DateTime end, string groupPeriod) where TMeasurement : class, IMeasurement;
        public IAsyncEnumerable<TMeasurement> GetEntriesBetween<TMeasurement>(string bucket, string measurement, DateTime start, DateTime end) where TMeasurement : class, IMeasurement;
        public IAsyncEnumerable<TMeasurement> GetEntriesBetween<TMeasurement>(string bucket, string measurement, string providerName, DateTime start, DateTime end) where TMeasurement : class, IMeasurement;
        public IAsyncEnumerable<TMeasurement> GetEntriesForPeriod<TMeasurement>(string bucket, string measurement, TimeInterval period) where TMeasurement : class, IMeasurement;
        public IAsyncEnumerable<TMeasurement> GetEntriesForPeriod<TMeasurement>(string bucket, string measurement, string providerName, TimeInterval period) where TMeasurement : class, IMeasurement;
        public Task<IEnumerable<T>> QueryAsync<T>(string query, IDomainObjectMapper? mapper = null) where T : class, IMeasurement;
        public Task<IEnumerable<TMeasurement>> GetEntriesBetweenAsync<TMeasurement>(string bucket, string measurement, DateTime start, DateTime end, string groupPeriod)
           where TMeasurement : class, IMeasurement;
        public Task<IEnumerable<TMeasurement>> GetEntriesBetweenAsync<TMeasurement>(string bucket, string measurement, string providerName, DateTime start, DateTime end) where TMeasurement : class, IMeasurement;
        public IAsyncEnumerable<T> QueryAsyncEnumerable<T>(string query) where T : class, IMeasurement;
    }
}
