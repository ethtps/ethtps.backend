using ETHTPS.Data.Core;

namespace ETHTPS.Data.Integrations.InfluxIntegration
{
    /// <summary>
    /// Provides InfluxDB write operations
    /// </summary>
    public interface IInfluxWriter
    {
        Task LogAsync<T>(T[] entries, string bucket)
            where T : class, IMeasurement;
        Task LogAsync<T>(T entry, string bucket)
            where T : class, IMeasurement;
        Task LogAsync<T>(T entry)
            where T : class, IMeasurement => LogAsync(entry, "init");
        Task CreateBucketAsync(string bucket);
    }
}
