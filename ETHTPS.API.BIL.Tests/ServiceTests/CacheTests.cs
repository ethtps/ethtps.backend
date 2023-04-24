using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.Data.Core;

using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Tests.ServiceTests
{
    [TestFixture]
    public sealed class CacheTests : TestBase
    {
        private ICachedDataService _cachedDataService;

        [SetUp]
        public void Setup()
        {
            // Initialize the ICachedDataService implementation here.
            _cachedDataService = ServiceProvider.GetRequiredService<ICachedDataService>();
        }

        [Test]
        public async Task HasDataAsync_ShouldReturnFalse_WhenDataDoesNotExist()
        {
            // Arrange
            string key = "non-existent-key";

            // Act
            bool result = await _cachedDataService.HasDataAsync(key);

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
            await _cachedDataService.SetDataAsync(key, expectedValue);

            // Act
            int? result = await _cachedDataService.GetDataAsync<int>(key);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public async Task SetDataAsync_ShouldSetData_WhenObjectImplementsICachedKey()
        {
            // Arrange
            TestCachedKey obj = new TestCachedKey();
            obj.Id = 1;
            obj.Name = "Test Object";

            // Act
            bool result = await _cachedDataService.SetDataAsync(obj);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
public class TestCachedKey : ICachedKey
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string ToCacheKey()
    {
        return $"test-key-{Id}";
    }
}