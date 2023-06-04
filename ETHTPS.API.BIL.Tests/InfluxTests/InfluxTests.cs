using ETHTPS.Data.Core;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Integrations.InfluxIntegration;

using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Tests.InfluxTests
{
    public sealed class InfluxTests : TestBase
    {
        private IInfluxWrapper? _influxWrapper;
        private IAsyncHistoricalBlockInfoProvider? _asyncHistoricalBlockInfoProvider;
        private const string _DEFAULT_BUCKET_NAME = "blockinfo";
        private const string _DEFAULT_MEASUREMENT_NAME = "blockinfo";

        [SetUp]
        public void Setup()
        {
            _influxWrapper = ServiceProvider.GetRequiredService<IInfluxWrapper>();
            _asyncHistoricalBlockInfoProvider = ServiceProvider.GetRequiredService<IAsyncHistoricalBlockInfoProvider>();
        }

        [Test]
        public void DependencyInjectionTest()
        {
            Assert.NotNull(_influxWrapper);
            Assert.Pass();
        }

        [Test]
        public void NoExceptionThrownTest()
        {
            if (_influxWrapper == null)
                return;

            Assert.DoesNotThrowAsync(async () =>
            {
                await foreach (var entry in _influxWrapper.GetEntriesBetween<Block
                    >(_DEFAULT_BUCKET_NAME, _DEFAULT_MEASUREMENT_NAME, DateTime.Now.Subtract(TimeSpan.FromMinutes(1)), DateTime.Now)) { }
            });
            Assert.DoesNotThrowAsync(async () =>
            {
                await foreach (var entry in _influxWrapper.GetEntriesForPeriod<Block
                    >(_DEFAULT_BUCKET_NAME, _DEFAULT_MEASUREMENT_NAME, TimeInterval.OneHour)) { }
            });
            /*
            Assert.DoesNotThrowAsync(async () =>
            {
                await foreach (var entry in _asyncHistoricalBlockInfoProvider.GetLatestBlocksAsync(new Data.Models.Query.ProviderQueryModel()
                {
                    Provider = "Ethereum",
                    Network = "Mainnet"
                }, Data.Core.TimeInterval.OneHour))
                { }
            });*/
            Assert.Pass();
        }

        [Test]
        public async Task ValuesOkAsync()
        {
            if (_asyncHistoricalBlockInfoProvider == null)
                return;

            var x = await _asyncHistoricalBlockInfoProvider.GetLatestBlocksAsync(new ProviderQueryModel(), TimeInterval.OneWeek);
            Assert.That(x.Any(x => x.TransactionCount > 0));
            Assert.Pass();
        }
    }
}