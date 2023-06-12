using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Integrations.InfluxIntegration;

using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Tests.InfluxTests
{
    [TestFixture]
    public sealed class InfluxValueTests : TestBase
    {
        private IInfluxWrapper? _influxWrapper;
        private IAggregatedDataService? _aggregatedDataservice;
        private IAsyncHistoricalBlockInfoProvider? _asyncHistoricalBlockInfoProvider;
        private const string _DEFAULT_BUCKET_NAME = "blockinfo";
        private const string _DEFAULT_MEASUREMENT_NAME = "blockinfo";
        private TimeInterval _interval = TimeInterval.OneDay;
        private DataType _dataType = DataType.TPS;
        private L2DataRequestModel? _requestModel;
        private static readonly string[] _TEST_PROVIDERS = new string[]
        {
            "Ethereum","Arbitrum One", "Aurora", "Celo", "Gnosis", "Loopring", "Metis", "Optimism", "Palm", "Polygon", "ZKSpace", "ZKSync", "ZKSync Era"
        };

        [SetUp]
        public void SetUp()
        {

            _requestModel = new L2DataRequestModel
            {
                StartDate = DateTime.Now.AddDays(-7),
                EndDate = DateTime.Now,
                Providers = _TEST_PROVIDERS.ToList()
            };
            _influxWrapper = ServiceProvider.GetRequiredService<IInfluxWrapper>();
            _asyncHistoricalBlockInfoProvider = ServiceProvider.GetRequiredService<IAsyncHistoricalBlockInfoProvider>();
            _aggregatedDataservice = ServiceProvider.GetRequiredService<IAggregatedDataService>();
        }

        [Test]
        public void GetDataAsync_ShouldReturnDataWithinRequestedTimeRange()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                if (_aggregatedDataservice == null || _requestModel == null)
                {
                    Assert.Fail($"{nameof(_requestModel)} or {nameof(_aggregatedDataservice)} are null");
                    return;
                }
                var response = await _aggregatedDataservice.GetDataAsync(_requestModel, _dataType);
                Assert.IsNotNull(response);
                var first = response.First().Data.First().Date;
                Assert.IsNotNull(first);
                Assert.That(first, Is.GreaterThanOrEqualTo(_requestModel?.StartDate));
                Assert.That(first, Is.LessThanOrEqualTo(_requestModel?.EndDate));
            });
        }
    }
}
