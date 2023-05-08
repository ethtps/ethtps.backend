using ETHTPS.Services.Ethereum;

using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Tests.ProviderTests
{
    public abstract class ProviderTestBase<T> : TestBase
        where T : BlockInfoProviderBase
    {
        private T _provider;

        [SetUp]
        public void SetUp()
        {
            _provider = ServiceProvider.GetRequiredService<T>();
        }

        [Test]
        public void LatestBlockDoesNotThrow_Test()
        {
            Assert.DoesNotThrowAsync(async () => await _provider.GetLatestBlockInfoAsync());
        }

        [Test]
        public void GetBlockInfoAsyncDoesNotThrow_Test()
        {
            Assert.DoesNotThrowAsync(async () => await _provider.GetBlockInfoAsync((await _provider.GetLatestBlockInfoAsync()).BlockNumber));
        }

        [Test]
        public void GetBlockInfoByDateAsyncDoesNotThrow_Test()
        {
            Assert.DoesNotThrowAsync(async () => await _provider.GetBlockInfoAsync((await _provider.GetLatestBlockInfoAsync()).Date));
        }
    }
}
