using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints.XYPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;

using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Tests.Data.DataTransformation
{
    public sealed class DataTransformationTests : TestBase
    {
        private IAggregatedDataService? _dataService;
        private readonly L2DataRequestModel _defaultModel = new L2DataRequestModel()
        {
            Provider = "Ethereum",
            StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)),
            EndDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
            ReturnXAxisType = XPointType.Date,
            Network = "Mainnet",
            BucketOptions = new BucketOptions()
            {
                UseTimeBuckets = true,
                CustomBucketSize = TimeSpan.FromHours(2),
            },
            IncludeSidechains = true,
        };

        [SetUp]
        public void Setup()
        {
            _dataService = ServiceProvider.GetRequiredService<IAggregatedDataService>();
        }

        [Test]
        public void NotNullTest()
        {
            Assert.That(_dataService != null);
        }

        [Test]
        public void DoesNotThrowTest()
        {

            Assert.DoesNotThrowAsync(async () =>
            {
                if (_dataService != null)
                    await _dataService.GetDataAsync(_defaultModel, DataType.TPS, TimeInterval.OneWeek);
            });
            Assert.DoesNotThrowAsync(async () =>
            {
                if (_dataService != null)
                    await _dataService.GetDataAsync(_defaultModel, DataType.GPS, TimeInterval.OneWeek);
            });
            Assert.DoesNotThrowAsync(async () =>
            {
                if (_dataService != null)
                    await _dataService.GetDataAsync(_defaultModel, DataType.GasAdjustedTPS, TimeInterval.OneWeek);
            });
        }
    }
}
