using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Integrations.InfluxIntegration;

using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Tests.InfluxTests
{
    [TestFixture]
    [Category("Database")]
    [Category("InfluxDB")]
    [Category("Essential")]
    public sealed class InfluxTests : TestBase
    {
        private IInfluxWrapper? _influxWrapper;
        private IAggregatedDataService? _aggregatedDataservice;
        private IAsyncHistoricalBlockInfoProvider? _asyncHistoricalBlockInfoProvider;
        private const string _DEFAULT_BUCKET_NAME = "blockinfo";
        private const string _DEFAULT_MEASUREMENT_NAME = "blockinfo";
        private static readonly string[] _TEST_PROVIDERS = new string[]
        {
            "Ethereum","Arbitrum One", "Aurora", "Celo", "Gnosis", "Loopring", "Metis", "Optimism", "Palm", "Polygon", "ZKSpace", "ZKSync", "ZKSync Era"
        };
        [SetUp]
        public void Setup()
        {
            _influxWrapper = ServiceProvider.GetRequiredService<IInfluxWrapper>();
            _asyncHistoricalBlockInfoProvider = ServiceProvider.GetRequiredService<IAsyncHistoricalBlockInfoProvider>();
            _aggregatedDataservice = ServiceProvider.GetRequiredService<IAggregatedDataService>();
        }

        [Test]
        public void DependencyInjectionTest()
        {
            Assert.NotNull(_influxWrapper);
            Assert.NotNull(_aggregatedDataservice);
            Assert.Pass();
        }

        [Test]
        public void NoExceptionThrownTest()
        {
            if (_influxWrapper == null)
                return;

            Assert.DoesNotThrowAsync(async () =>
            {
                if (!await _influxWrapper.BucketExistsAsync(_DEFAULT_BUCKET_NAME))
                    await _influxWrapper.CreateBucketAsync(_DEFAULT_BUCKET_NAME);

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
        public void NoErrorThrown()
        {
            if (_aggregatedDataservice == null)
                return;

            Assert.DoesNotThrowAsync(async () =>
            {
                var x = await _aggregatedDataservice.GetTPSAsync(new ProviderQueryModel()
                {
                    Provider = "All"
                }, TimeInterval.OneWeek);
                Assert.That(x.Any(x => x.Data
                .Sum(g => g.Value) > 0));
            }, "The sum of everything is zero. Either the database is empty or something's wrong with the query or the implementation.");
        }
    }
}