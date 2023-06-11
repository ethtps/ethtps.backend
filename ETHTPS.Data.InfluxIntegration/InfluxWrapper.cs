
using System.Diagnostics;

using ETHTPS.Configuration;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.Extensions;
using ETHTPS.Data.Integrations.InfluxIntegration.Extensions;

using Flux.Net;

using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;

using Microsoft.Extensions.Logging;

namespace ETHTPS.Data.Integrations.InfluxIntegration
{
    /// <summary>
    /// Calling it a wrapper so we don't conflict with the library
    /// </summary>
    public sealed class InfluxWrapper : IInfluxWrapper
    {
        private readonly InfluxWrapperConfiguration _configuration;
        private readonly InfluxDBClient _influxClient;
        private readonly WriteApiAsync _writeApi;
        private readonly QueryApi _queryApi;
        private readonly BucketsApi _bucketsApi;
        private readonly ILogger<InfluxWrapper> _logger;
        private readonly Stopwatch _stopwatch = new();

        public InfluxWrapper(IDBConfigurationProvider configurationProvider, ILogger<InfluxWrapper> logger) : this(InfluxWrapperConfiguration.FromConfigurationProvider(configurationProvider), logger)
        {

        }

        public InfluxWrapper(InfluxWrapperConfiguration configuration, ILogger<InfluxWrapper> logger)
        {
            _configuration = configuration;
            _influxClient = new InfluxDBClient(new InfluxDBClientOptions(_configuration.URL)
            {
                Org = _configuration.Org,
                Token = _configuration.Token,
                Username = _configuration.Username,
                Password = _configuration.Password,
                //Bucket = _configuration.Bucket,
            });
            _writeApi = _influxClient.GetWriteApiAsync();
            _bucketsApi = _influxClient.GetBucketsApi();
            _queryApi = _influxClient.GetQueryApi();
            _logger = logger;
        }

        private async Task WaitForClientAsync()
        {
            int c = 0;
            while (await _influxClient.ReadyAsync() == null)
            {
                _logger.LogInformation($"[{++c}] Waiting for client...");
                await Task.Delay(2500);
            }
        }

        public async Task<bool> BucketExistsAsync(string name)
        {
            await WaitForClientAsync();
            return await _bucketsApi.FindBucketByNameAsync(name) != default(Bucket);
        }

        public async Task CreateBucketAsync(string name)
        {
            await WaitForClientAsync();
            await _bucketsApi.CreateBucketAsync(name, _configuration.OrgID);
            _logger.LogInformation($"Created InfluxDB bucket [{_configuration.OrgID}].[{name}]");
        }

        public async Task<IEnumerable<string>> GetBucketsAsync() => (await _bucketsApi.FindBucketsAsync(org: _configuration.Org))?.Select(x => x.Name) ?? Enumerable.Empty<string>();

        public async Task LogAsync<T>(T entry, string bucket = "")
            where T : class, IMeasurement
        {
            try
            {
                _stopwatch.Restart();
                CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromSeconds(15));
                await WaitForClientAsync();
                var response = await _writeApi.WriteMeasurementsAsyncWithIRestResponse(new[]
                {
                    entry
                }, WritePrecision.Ms, bucket ?? _configuration.Bucket, _configuration.Org, cancellationTokenSource.Token);
                _stopwatch.Stop();
                if (!response.IsSuccessful)
                {
                    throw new Exception("Error writing to InfluxDB: " + response.ErrorMessage);
                }
                else
                {
                    _logger.LogInformation($"Logged {typeof(T).Name} {entry.ToString()} in {_stopwatch.Elapsed.TotalMilliseconds}ms");
                }
            }
            catch (Exception e)
            {
                throw new InfluxException("Influx write failed", e);
            }
        }

        public async Task DeleteBucketAsync(string name)
        {
            await WaitForClientAsync();
            await DeleteAllDataInBucketAsync(name);
            _logger.LogInformation($"Deleting bucket {name}...");
            await _bucketsApi.DeleteBucketAsync(name);
            _logger.LogInformation("Done");
        }

        public async Task DeleteAllBucketsAsync()
        {
            var buckets = await GetBucketsAsync();
            foreach (var bucket in buckets)
            {
                await DeleteBucketAsync(bucket);
            }
        }

        /// <summary>
        /// Can't set the ID of the organization because it is private for some reason so we have to hack it; *puts sunglasses on*
        /// </summary>
        class OrganizationHack : Organization
        {
            public OrganizationHack() : base() { }
            public static Organization HackOrganization(string id)
            {
                OrganizationHack result = new();
                typeof(Organization)?.GetProperty("Id")?.SetValue(result, id);
                return result;
            }
        }

        public async Task DeleteAllDataInBucketAsync(string bucket)
        {
            await WaitForClientAsync();
            _logger.LogInformation($"Deleting all data in bucket {bucket}...");
            await _influxClient.GetDeleteApi().Delete(DateTime.Now.Subtract(TimeSpan.FromDays(30)), DateTime.Now, $"_measurement=\"{bucket.ClearBucketNameSuffix().ToLower()}\"", await _bucketsApi.FindBucketByNameAsync(bucket), OrganizationHack.HackOrganization(_configuration.OrgID ?? string.Empty));
            _logger.LogInformation("Done");
        }

        public async Task LogAsync<T>(T[] entries, string bucket) where T : class, IMeasurement
        {
            await WaitForClientAsync();
            try
            {
                _stopwatch.Restart();
                CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromSeconds(15));
                await WaitForClientAsync();
                var response = await _writeApi.WriteMeasurementsAsyncWithIRestResponse(entries, WritePrecision.Ms, bucket ?? _configuration.Bucket, _configuration.Org, cancellationTokenSource.Token);
                _stopwatch.Stop();
                if (!response.IsSuccessful)
                {
                    throw new Exception("Error writing to InfluxDB: " + response.ErrorMessage);
                }
                else
                {
                    _logger.LogInformation($"Logged [{typeof(T).Name}] of length {entries.Length} in {_stopwatch.Elapsed.TotalMilliseconds}ms");
                }
            }
            catch (Exception e)
            {
                throw new InfluxException("Influx write failed", e);
            }
        }

        public async IAsyncEnumerable<T> GetEntriesBetween<T>(string bucket, string measurement, DateTime start, DateTime end) where T : class, IMeasurement
        {
            var query = QueryBuilder.From(bucket)
                            .Filter(x => x.Measurement(measurement))
                            .AbsoluteTimeRange(start, end)
                            .ToCleanFluxQuery();
            await WaitForClientAsync();
            await foreach (var entry in _queryApi.QueryAsyncEnumerable<T>(query))
            {
                yield return entry;
            }
        }

        public IAsyncEnumerable<T> GetEntriesForPeriod<T>(string bucket, string measurement, TimeInterval period) where T : class, IMeasurement => GetEntriesBetween<T>(bucket, measurement, DateTime.Now - period.ExtractTimeGrouping().ToTimeSpan(), DateTime.Now);

        public async IAsyncEnumerable<T> QueryAsyncEnumerable<T>(string query)
             where T : class, IMeasurement
        {
            await WaitForClientAsync();
            await foreach (var entry in _queryApi.QueryAsyncEnumerable<T>(query))
            {
                yield return entry;
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query, IDomainObjectMapper? mapper)
            where T : class, IMeasurement
        {
            await WaitForClientAsync();
            var table = await _influxClient.GetQueryApi().QueryAsync(query, typeof(T), org: _configuration.Org);
            return await _queryApi.QueryAsync<T>(query);
        }

        public async IAsyncEnumerable<TMeasurement> GetEntriesBetween<TMeasurement>(string bucket, string measurement, DateTime start, DateTime end, string groupPeriod)
            where TMeasurement : class, IMeasurement
        {
            var query = $"from(bucket: \"{bucket}\")\r\n  |> range(start: {start.ToInfluxDateTime()}, stop: {end.ToInfluxDateTime()})\r\n  |> filter(fn: (r) => r[\"_measurement\"] == \"{measurement}\")\r\n  |> filter(fn: (r) => r[\"_field\"] == \"gasused\" or r[\"_field\"] == \"blocknumber\" or r[\"_field\"] == \"transactioncount\")\r\n  |> aggregateWindow(every: {groupPeriod}, fn: mean, createEmpty: false)\r\n  |> yield(name: \"mean\")";
            await WaitForClientAsync();
            await foreach (var entry in _queryApi.QueryAsyncEnumerable<TMeasurement>(query))
            {
                yield return entry;
            }
        }

        public async IAsyncEnumerable<TMeasurement> GetEntriesBetween<TMeasurement>(string bucket, string measurement, string providerName, DateTime start, DateTime end)
                where TMeasurement : class, IMeasurement
        {
            var query = $"from(bucket: \"{bucket}\")\r\n  |> range(start: {start.ToInfluxDateTime()}, stop: {end.ToInfluxDateTime()})\r\n  |> filter(fn: (r) => r[\"_measurement\"] == \"{measurement}\")\r\n  |> filter(fn: (r) => r[\"_field\"] == \"gasused\" or r[\"_field\"] == \"blocknumber\" or r[\"_field\"] == \"transactioncount\")\r\n   |> filter(fn: (r) => r[\"provider\"] == \"{providerName}\")\r\n  |> aggregateWindow(every: auto, fn: mean, createEmpty: false)\r\n  |> yield(name: \"mean\")";
            await WaitForClientAsync();
            await foreach (var entry in _queryApi.QueryAsyncEnumerable<TMeasurement>(query))
            {
                yield return entry;
            }
        }

        public IAsyncEnumerable<TMeasurement> GetEntriesForPeriod<TMeasurement>(string bucket, string measurement, string providerName, TimeInterval period)
            where TMeasurement : class, IMeasurement => GetEntriesBetween<TMeasurement>(bucket, measurement, providerName, DateTime.Now - period.ExtractTimeGrouping().ToTimeSpan(), DateTime.Now);
    }
}
