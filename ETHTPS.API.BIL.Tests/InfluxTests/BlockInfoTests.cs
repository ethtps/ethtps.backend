using ETHTPS.API.BIL.Infrastructure.Services.DataServices.TPS;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Integrations.InfluxIntegration;

using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Tests.InfluxTests
{
    public sealed class BlockInfoTests : TestBase
    {
        private IInfluxWrapper? _influxWrapper;
        private IAsyncHistoricalBlockInfoProvider? _asyncHistoricalBlockInfoProvider;
        private const string _DEFAULT_BUCKET_NAME = "blockinfo";
        private const string _DEFAULT_MEASUREMENT_NAME = "blockinfo";
        private ITPSService? _tpsService;

        [SetUp]
        public void Setup()
        {
            _influxWrapper = ServiceProvider.GetRequiredService<IInfluxWrapper>();
            _asyncHistoricalBlockInfoProvider = ServiceProvider.GetRequiredService<IAsyncHistoricalBlockInfoProvider>();
            _tpsService = ServiceProvider.GetRequiredService<ITPSService>();
        }

        [Test]
        public void DependencyInjectionTest()
        {
            Assert.NotNull(_tpsService);
            Assert.Pass();
        }

        [Test]
        public void TimeIntervalTests()
        {
            if (_tpsService == null)
            {
                Assert.Fail();
                return;
            }
            Assert.DoesNotThrowAsync(async () =>
            {
                await _tpsService.GetAsync(ProviderQueryModel.FromProviderName("Ethereum"), TimeInterval.OneDay);
            });
        }


        [Test]
        public async Task OnlyRequestedProvidersAreReturnedTest()
        {
            if (_tpsService == null)
            {
                Assert.Fail();
                return;
            }
            var result = await _tpsService.GetAsync(ProviderQueryModel.FromProviderName("Ethereum"), TimeInterval.OneYear);
            Assert.That(result.Count, Is.AtMost(1));
            Assert.IsTrue(result.ContainsKey("Ethereum"));
        }
    }
}