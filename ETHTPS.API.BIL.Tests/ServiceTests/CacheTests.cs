using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.API.Core.Services;
using ETHTPS.Configuration;
using ETHTPS.Configuration.Extensions;
using ETHTPS.Data.Core;

using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis;

namespace ETHTPS.Tests.ServiceTests
{
    [TestFixture]
    public sealed class CacheTests : TestBase
    {
        private IConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;
        private IRedisCacheService _cachedDataService;

        [SetUp]
        public void Setup()
        {
            // Initialize the ICachedDataService implementation here.
            _connectionMultiplexer = ConnectionMultiplexer.Connect(ServiceProvider.GetRequiredService<IDBConfigurationProvider>().GetFirstConfigurationString("RedisServer") ?? "localhost");
            _database = _connectionMultiplexer.GetDatabase();
            _cachedDataService = new RedisCachedDataService(_connectionMultiplexer);
        }

        [TearDown]
        public void TearDown()
        {
            // Flush the Redis database after each test
            _database.Execute("FLUSHALL");
        }

        [Test]
        public async Task HasDataAsync_ShouldReturnFalse_WhenDataDoesNotExist()
        {
            // Arrange
            string key = "non-existent-key";

            // Act
            bool result = await _cachedDataService.HasKeyAsync(key);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task SetDataAsync_ShouldSetData_WhenValidKeyAndValueProvided()
        {
            // Arrange
            string key = "valid-key";
            string value = "valid-value";

            // Act
            bool result = await _cachedDataService.SetDataAsync(key, value);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task SetDataAsync_ShouldSetData_WhenValidKeyAndValueProvided_ForGenericMethod()
        {
            // Arrange
            string key = "valid-key";
            int value = 42;

            // Act
            bool result = await _cachedDataService.SetDataAsync(key, value);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetDataAsync_ShouldReturnData_WhenDataExists_ForGenericMethod()
        {
            // Arrange
            string key = "valid-key";
            int expectedValue = 42;
            _ = await _cachedDataService.SetDataAsync(key, expectedValue);

            // Act
            int? result = await _cachedDataService.GetDataAsync<int>(key);

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        [Test]
        public async Task SetDataAsync_ShouldSetData_WhenObjectImplementsICachedKey()
        {
            // Arrange
            TestCachedKey obj = new()
            {
                Id = 1,
                Name = "Test Object"
            };

            // Act
            bool result = await _cachedDataService.SetDataAsync(obj);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task HasDataAsync_ShouldReturnTrue_WhenDataExists()
        {
            // Arrange
            string key = "valid-key";
            _ = await _cachedDataService.SetDataAsync(key, "valid-value");

            // Act
            bool result = await _cachedDataService.HasKeyAsync(key);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task SetDataAsync_ShouldReturnFalse_WhenKeyIsNull()
        {
            // Arrange
            string? key = null;
            string value = "valid-value";

            // Act
            bool result = await _cachedDataService.SetDataAsync(key, value);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task SetDataAsync_ShouldReturnFalse_WhenValueIsNull()
        {
            // Arrange
            string key = "valid-key";
            string? value = null;

            // Act
            bool result = await _cachedDataService.SetDataAsync(key, value);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task SetDataAsync_ShouldReturnFalse_WhenKeyIsWhitespace()
        {
            // Arrange
            string key = "   ";
            string value = "valid-value";

            // Act
            bool result = await _cachedDataService.SetDataAsync(key, value);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task SetDataAsync_ShouldReturnFalse_WhenValueIsWhitespace()
        {
            // Arrange
            string key = "valid-key";
            string value = "   ";

            // Act
            bool result = await _cachedDataService.SetDataAsync(key, value);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetDataAsync_ShouldReturnNull_WhenDataDoesNotExist()
        {
            // Arrange
            string key = "non-existent-key";

            // Act
            int? result = await _cachedDataService.GetDataAsync<int>(key);

            // Assert
            Assert.That(result, Is.EqualTo(default(int)));
        }
        [Test]
        public async Task SetDataAsync_WithTTL_ShouldSetDataWithTTL()
        {
            // Arrange
            string key = "testshouldsetkey";
            string value = "testvalue";
            TimeSpan ttl = TimeSpan.FromSeconds(10);

            // Act
            bool result = await _cachedDataService.SetDataAsync(key, value, ttl);

            // Assert
            Assert.True(result);
            Assert.True(await _database.KeyExistsAsync(key));
            Assert.True(await _database.StringGetAsync(key) == value);
            Assert.LessOrEqual(await _database.KeyTimeToLiveAsync(key), ttl);
        }

        [Test]
        public async Task SetDataAsync_WithNegativeTTL_ShouldNotSetDataWithTTL()
        {
            // Arrange
            string key = "testnegttlkey";
            string value = "testvalue";
            TimeSpan ttl = TimeSpan.FromSeconds(-1);

            // Act
            bool result = await _cachedDataService.SetDataAsync(key, value, ttl);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(await _database.KeyExistsAsync(key), Is.False);
        }

        [Test]
        public async Task SetDataAsync_WithZeroTTL_ShouldNotSetDataWithTTL()
        {
            // Arrange
            string key = "testzerottlkey";
            string value = "testvalue";
            TimeSpan ttl = TimeSpan.Zero;

            // Act
            bool result = await _cachedDataService.SetDataAsync(key, value, ttl);

            // Assert
            Assert.False(result);
            Assert.False(await _database.KeyExistsAsync(key));
        }

        [Test]
        public async Task GetDataAsync_WithExpiredTTL_ShouldNotGetData()
        {
            // Arrange
            string key = "testkey";
            string value = "testvalue";
            TimeSpan ttl = TimeSpan.FromSeconds(1);

            _ = await _cachedDataService.SetDataAsync(key, value, ttl);

            // Wait for TTL to expire
            await Task.Delay(ttl.Add(TimeSpan.FromSeconds(1)));

            // Act
            string? result = await _cachedDataService.GetDataAsync<string>(key);

            // Assert
            Assert.IsNull(result);
        }
    }
}
public class TestCachedKey : ICachedKey
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public string ToCacheKey()
    {
        return $"test-key-{Id}";
    }
}