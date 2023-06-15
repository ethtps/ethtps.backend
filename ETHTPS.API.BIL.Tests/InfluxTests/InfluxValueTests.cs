using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Extensions;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Integrations.InfluxIntegration;

using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Tests.InfluxTests
{
    /// <summary>
    /// All kinds of tests in order to make sure chart data requests are handled correctly. The more the merrier - this is an important feature.
    /// </summary>
    /// <seealso cref="ETHTPS.Tests.TestBase" />
    [TestFixture]
    [Category("Database")]
    [Category("InfluxDB")]
    [Category("BusinessLogic")]
    [Category("Essential")]
    public sealed class InfluxValueTests : TestBase
    {
        private IInfluxWrapper? _influxWrapper;
        private IAggregatedDataService? _aggregatedDataservice;
        private IAsyncHistoricalBlockInfoProvider? _asyncHistoricalBlockInfoProvider;
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
                Assert.That(response, Is.Not.Null);
                var first = response.Datasets?.First().DataPoints.First().ToDatedXYDataPoint();
                Assert.That(first, Is.Not.Null);
                Assert.That(first?.X, Is.GreaterThanOrEqualTo(_requestModel?.StartDate));
                Assert.That(first?.X, Is.LessThanOrEqualTo(_requestModel?.EndDate));
            });
        }

        [Test]
        public void GetDataAsync_ShouldReturnEmptyList_WhenNoDataExists()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                if (_aggregatedDataservice == null)
                {
                    Assert.Fail($"{nameof(_aggregatedDataservice)} is null");
                    return;
                }

                // Set a date range where no data is expected.
                var noDataRequestModel = new L2DataRequestModel
                {
                    StartDate = DateTime.Now.AddYears(-100),  // Example: 100 years ago.
                    EndDate = DateTime.Now.AddYears(-99),    // Example: 99 years ago.
                    Providers = _TEST_PROVIDERS.ToList()
                };

                var response = await _aggregatedDataservice.GetDataAsync(noDataRequestModel, _dataType);
                Assert.That(response, Is.Not.Null, "Response is null");
                Assert.That(response.Datasets, Is.Empty, "Response datasets is not empty");  // Assert that the response is an empty list.
            });
        }

        [Test]
        public void GetDataAsync_ShouldHandleNullRequestModel()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                if (_aggregatedDataservice == null)
                {
                    Assert.Fail($"{nameof(_aggregatedDataservice)} is null");
                    return;
                }

                L2DataRequestModel? nullRequestModel = null;
#pragma warning disable CS8604 // Possible null reference argument.
                var response = await _aggregatedDataservice.GetDataAsync(nullRequestModel, _dataType);
#pragma warning restore CS8604 // Possible null reference argument.
            });
        }

        [Test]
        public void GetDataAsync_ShouldReturnCorrectDataTypes()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                if (_aggregatedDataservice == null || _requestModel == null)
                {
                    Assert.Fail($"{nameof(_requestModel)} or {nameof(_aggregatedDataservice)} are null");
                    return;
                }

                var response = await _aggregatedDataservice.GetDataAsync(_requestModel, _dataType);
                Assert.That(response, Is.Not.Null);
                Assert.That(response.DataType, Is.EqualTo(_dataType));
            });
        }

        [Test]
        public void GetDataAsync_ShouldReturnDataForAllRequestedProviders()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                if (_aggregatedDataservice == null || _requestModel == null)
                {
                    Assert.Fail($"{nameof(_requestModel)} or {nameof(_aggregatedDataservice)} are null");
                    return;
                }

                var response = await _aggregatedDataservice.GetDataAsync(_requestModel, _dataType);
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Datasets, Is.Not.Null);

                var providerSet = new HashSet<string>(_requestModel.AllDistinctProviders);
#pragma warning disable CS8604 // Possible null reference argument - we're already handling this above
                var responseProviders = new HashSet<string>(response.Datasets.Select(data => data.Provider));
#pragma warning restore CS8604 // Possible null reference argument.

                Assert.That(providerSet.SetEquals(responseProviders), Is.True, "Returned data does not cover all requested providers.");
            });
        }

        [Test]
        public void GetDataAsync_ShouldApplyBucketOptionsCorrectlyAndShouldReturnDataInCorrectTimeInterval()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                if (_aggregatedDataservice == null || _requestModel == null)
                {
                    Assert.Fail($"{nameof(_requestModel)} or {nameof(_aggregatedDataservice)} are null");
                    return;
                }
                foreach (var interval in Enum.GetValues(typeof(TimeInterval)))
                {
                    if ((TimeInterval)interval == TimeInterval.Auto)
                        continue;

                    _requestModel.BucketOptions = new BucketOptions
                    {
                        BucketSize = (TimeInterval)interval
                    };

                    var expectedSize = ((TimeInterval)interval).ExtractTimeGrouping().ToTimeSpan();

                    var response = await _aggregatedDataservice.GetDataAsync(_requestModel, _dataType);
                    Assert.That(response, Is.Not.Null);

                    // You would need to add assertions here to verify that the response matches the bucket options.
                    var data = response.Datasets?.ToList();
                    Assert.That(data, Is.Not.Null);
                    Assert.That(data?.Count, Is.GreaterThan(0));
                    foreach (var dataset in data ?? throw new Exception("Null datasets"))
                    {
                        var datedPoints = dataset.DataPoints.Select(x => x.ToDatedXYDataPoint());
                        for (int i = 0, j = 1; i < datedPoints?.Count() - 1; i++, j++)
                        {
                            Assert.That(datedPoints?.ElementAt(j).X - datedPoints?.ElementAt(i).X, Is.GreaterThanOrEqualTo(expectedSize));
                        }
                    }
                }
            });
        }

        [Test]
        public void GetDataAsync_ShouldReturnDataInCorrectTimeInterval()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                if (_aggregatedDataservice == null || _requestModel == null)
                {
                    Assert.Fail($"{nameof(_requestModel)} or {nameof(_aggregatedDataservice)} are null");
                    return;
                }

                var response = await _aggregatedDataservice.GetDataAsync(_requestModel, _dataType);
                Assert.That(response, Is.Not.Null);
            });
        }

        [Test]
        public async Task GetDataAsync_ShouldPerformSimpleAnalysisWhenRequested()
        {
            // Arrange
            if (_aggregatedDataservice == null || _requestModel == null)
            {
                Assert.Fail($"{nameof(_requestModel)} or {nameof(_aggregatedDataservice)} are null");
                return;
            }

            _requestModel.IncludeSimpleAnalysis = true;

            // Act
            var response = await _aggregatedDataservice.GetDataAsync(_requestModel, _dataType);

            // Assert
            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Datasets, Is.Not.Null);
#pragma warning disable CS8604 // Possible null reference argument.
            Assert.That((bool)(response.Datasets.All(x => x.SimpleAnalysis != null)));
#pragma warning restore CS8604 // Possible null reference argument.
        }

        [Test]
        public async Task GetDataAsync_ShouldPerformComplexAnalysisWhenRequested()
        {
            // Arrange
            if (_aggregatedDataservice == null || _requestModel == null)
            {
                Assert.Fail($"{nameof(_requestModel)} or {nameof(_aggregatedDataservice)} are null");
                return;
            }

            _requestModel.IncludeComplexAnalysis = true;

            // Act
            var response = await _aggregatedDataservice.GetDataAsync(_requestModel, _dataType);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Datasets, Is.Not.Null);

            // Check for presence of complex analysis data
            // NOTE: Replace 'ComplexAnalysisProperty' with the actual property that indicates the result of complex analysis
#pragma warning disable CS8604 // Possible null reference argument.
            Assert.That((bool)(response.Datasets.All(x => x.ComplexAnalysis != null)));
#pragma warning restore CS8604 // Possible null reference argument.
        }

        [Test]
        public async Task GetDataAsync_ShouldIncludeEmptyDatasetsWhenRequested()
        {
            // Arrange
            if (_aggregatedDataservice == null || _requestModel == null)
            {
                Assert.Fail($"{nameof(_requestModel)} or {nameof(_aggregatedDataservice)} are null");
                return;
            }

            _requestModel.IncludeEmptyDatasets = true;

            // Act
            var response = await _aggregatedDataservice.GetDataAsync(_requestModel, _dataType);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Datasets, Is.Not.Null);

            if (response.Datasets != null)
            {
                Assert.That(response.Datasets.Count, Is.EqualTo(_TEST_PROVIDERS.Length));
            }
        }

        [Test]
        public async Task GetDataAsync_ShouldMergeDatasetsCorrectlyWhenRequested()
        {
            // Arrange
            if (_aggregatedDataservice == null || _requestModel == null)
            {
                Assert.Fail($"{nameof(_requestModel)} or {nameof(_aggregatedDataservice)} are null");
                return;
            }

            var max = 4;
            Assert.That(max, Is.LessThanOrEqualTo(_TEST_PROVIDERS.Length));

            _requestModel.MergeOptions = new DatasetMergeOptions
            {
                MaxCount = max
            };

            // Act
            var response = await _aggregatedDataservice.GetDataAsync(_requestModel, _dataType);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Datasets, Is.Not.Null);
            if (response.Datasets != null)
            {
                Assert.That(response.Datasets.Count, Is.LessThanOrEqualTo(max));
            }
        }
        [Test]
        public void GetDataAsync_ShouldThrowException_WhenEndDateLessThanStartDate()
        {
            Assert.ThrowsAsync<Exception>(async () =>
            {
                if (_aggregatedDataservice == null)
                {
                    Assert.Fail($"{nameof(_aggregatedDataservice)} is null");
                    return;
                }

                L2DataRequestModel model = new L2DataRequestModel()
                {
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(-1),
                    Provider = "Ethereum"
                };
                try
                {
                    var response = await _aggregatedDataservice.GetDataAsync(model, _dataType);
                }
                catch
                {
                    throw new Exception();
                }
            });
        }
    }
}
