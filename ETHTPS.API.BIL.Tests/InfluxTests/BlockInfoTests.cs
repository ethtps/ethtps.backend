using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GTPS;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.TPS;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Integrations.InfluxIntegration.ProviderServices.DataProviders;

using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Tests.InfluxTests
{
    [TestFixture]
    [Category("GPS")]
    public sealed class InfluxGPSTests : InfluxBlockInfoTests<InfluxGPSService>
    {
        public InfluxGPSTests()
        {
            _service = ServiceProvider.GetRequiredService<InfluxGPSService>();
        }
    }

    [TestFixture]
    [Category("GTPS")]
    public sealed class InfluxGTPSTests : InfluxBlockInfoTests<InfluxGTPSService>
    {
        public InfluxGTPSTests()
        {
            _service = ServiceProvider.GetRequiredService<IGTPSService>();
        }
    }

    [TestFixture]
    [Category("TPS")]
    public sealed class InfluxTPSTests : InfluxBlockInfoTests<InfluxTPSService>
    {
        public InfluxTPSTests()
        {
            _service = ServiceProvider.GetRequiredService<ITPSService>();
        }
    }

    [Category("CoreFunctionality")]
    [Category("Essential")]
    public abstract class InfluxBlockInfoTests<T> : TestBase
        where T : InfluxPSServiceBase
    {
        protected IPSService? _service;

        [SetUp]
        public void Setup()
        {
            _service = ServiceProvider.GetRequiredService<T>();
        }

        [Test]
        public void DependencyInjectionTest()
        {
            Assert.NotNull(_service);
            Assert.Pass();
        }

        [Test]
        public void TimeIntervalTests()
        {
            if (_service == null)
            {
                Assert.Fail();
                return;
            }
            Assert.DoesNotThrowAsync(async () =>
            {
                await _service.GetAsync(ProviderQueryModel.FromProviderName("Ethereum"), TimeInterval.OneDay);
            });
        }


        [Test]
        public async Task OnlyRequestedProvidersAreReturnedTest()
        {
            if (_service == null)
            {
                Assert.Fail();
                return;
            }
            var result = await _service.GetAsync(ProviderQueryModel.FromProviderName("Ethereum"), TimeInterval.OneYear);
            Assert.That(result, Has.Count.AtMost(1));
            Assert.That(result.ContainsKey("Ethereum"), Is.True);
        }

        [Test]
        public void MaxWorksTest()
        {
            if (_service == null)
            {
                Assert.Fail();
                return;
            }

            Assert.DoesNotThrowAsync(async () =>
            {
                await _service.MaxAsync(ProviderQueryModel.FromProviderName("Ethereum"));
            });
        }

        [Test]
        public void MaxAllDoesNotThrowTest()
        {
            if (_service == null)
            {
                Assert.Fail();
                return;
            }

            Assert.DoesNotThrowAsync(async () =>
            {
                await _service.MaxAsync(ProviderQueryModel.FromProviderName("All"));
            });
        }

        [Test]
        public void InstantWorksTest()
        {
            if (_service == null)
            {
                Assert.Fail();
                return;
            }

            Assert.DoesNotThrowAsync(async () =>
            {
                await _service.InstantAsync(ProviderQueryModel.FromProviderName("Ethereum"));
            });
        }

        [Test]
        public void InstantAllDoesNotThrowTest()
        {
            if (_service == null)
            {
                Assert.Fail();
                return;
            }

            Assert.DoesNotThrowAsync(async () =>
            {
                await _service.InstantAsync(ProviderQueryModel.FromProviderName("All"));
            });
        }

        [Test]
        public void MonthlyByYearDoesNotThrow()
        {
            if (_service == null)
            {
                Assert.Fail();
                return;
            }
            Assert.DoesNotThrowAsync(async () =>
            {
                await _service.GetMonthlyDataByYearAsync(ProviderQueryModel.FromProviderName("Ethereum"), DateTime.Now.Year);
            });
        }
    }
}