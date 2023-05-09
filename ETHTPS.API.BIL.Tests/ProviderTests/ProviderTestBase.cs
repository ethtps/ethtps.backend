using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Services.Ethereum;

namespace ETHTPS.Tests.ProviderTests
{
    public abstract class ProviderTestBase<T> : TestBase
        where T : BlockInfoProviderBase
    {
        protected T? _provider;

        [SetUp]
        public abstract void SetUp();

        [Test]
        public void NotNullTest()
        {
            Assert.NotNull(_provider);
        }

        [Test]
        public void LatestBlock_NoException_ResultOk_Test()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Block? result;
            Assert.DoesNotThrowAsync(async () =>
            {
                result = await _provider.GetLatestBlockInfoAsync();
                Assert.NotNull(result);
            });
        }

        [Test]
        public void GetBlockInfoAsync_NoException_ResultOk_Test()
        {
            Block? result;
            Assert.DoesNotThrowAsync(async () =>
            {
                result = await _provider.GetBlockInfoAsync((await _provider.GetLatestBlockInfoAsync()).BlockNumber);
                Assert.NotNull(result);
            });
        }

        [Test]
        public void GetBlockInfoByDateAsync_NoException_ResultOk_Test()
        {
            Block? result;
            Assert.DoesNotThrowAsync(async () =>
            {
                result = await _provider.GetBlockInfoAsync((await _provider.GetLatestBlockInfoAsync()).Date);
                Assert.NotNull(result);
            });
        }
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.