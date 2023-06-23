using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.Data.Core.Models.DataUpdater;

using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Tests.ServiceTests
{
    [Category("Services")]
    [Category("DataUpdaters")]
    [Category("Essential")]
    [TestFixture]
    public sealed class DataUpdaterService : TestBase
    {
        private IDataUpdaterStatusService? _statusService;
        private const string _TEST_PROVIDER_NAME = "Ethereum";
        private const UpdaterType _TEST_UPDATERTYPE_NAME = UpdaterType.BlockInfo;
        private const UpdaterStatus _TEST_STATUS = UpdaterStatus.InTest;

        [SetUp]
        public void Setup()
        {
            _statusService = ServiceProvider.GetRequiredService<IDataUpdaterStatusService>();
        }

        [Test]
        public void DependencyInjectionTest()
        {
            Assert.NotNull(_statusService);
            Assert.Pass();
        }

        [Test]
        public void NoExceptionThrownTest()
        {
            Assert.DoesNotThrow(() =>
            {
                _statusService?.GetAllStatuses();
            });
            Assert.DoesNotThrow(() =>
            {
                _statusService?.GetStatusFor(_TEST_PROVIDER_NAME);
            });
            Assert.DoesNotThrow(() =>
            {
                _statusService?.GetStatusFor(_TEST_PROVIDER_NAME, _TEST_UPDATERTYPE_NAME);
            });
            Assert.DoesNotThrow(() =>
            {
                _statusService?.IncrementNumberOfFailures(_TEST_PROVIDER_NAME, _TEST_UPDATERTYPE_NAME);
            });
            Assert.DoesNotThrow(() =>
            {
                _statusService?.IncrementNumberOfSuccesses(_TEST_PROVIDER_NAME, _TEST_UPDATERTYPE_NAME);
            });
            Assert.DoesNotThrow(() =>
            {
                _statusService?.MarkAsRanSuccessfully(_TEST_PROVIDER_NAME, _TEST_UPDATERTYPE_NAME);
            });
            Assert.DoesNotThrow(() =>
            {
                _statusService?.MarkAsFailed(_TEST_PROVIDER_NAME, _TEST_UPDATERTYPE_NAME);
            });
            Assert.DoesNotThrow(() =>
            {
                _statusService?.SetStatusFor(_TEST_PROVIDER_NAME, _TEST_UPDATERTYPE_NAME, UpdaterStatus.RanSuccessfully);
            });
            _statusService?.SetStatusFor(_TEST_PROVIDER_NAME, _TEST_UPDATERTYPE_NAME, UpdaterStatus.Idle);
            Assert.Pass();
        }

        [Test]
        public void UniquenessTest()
        {
            var groups = _statusService?.GetAllStatuses().GroupBy(x => (x ?? ETHTPS.Data.Core.Models.DataUpdater.LiveDataUpdaterStatus.EMPTY).Updater + (x ?? ETHTPS.Data.Core.Models.DataUpdater.LiveDataUpdaterStatus.EMPTY).UpdaterType);
            if ((bool)(groups?.Any(x => x.Count() > 1)).GetValueOrDefault())
            {
                Assert.Fail("Multiple entries for the same updater found", groups?.Where(x => x.Count() > 1));
            }
            Assert.Pass();
        }

        [Test]
        public void StatusSetTest()
        {
            _statusService?.SetStatusFor(_TEST_PROVIDER_NAME, _TEST_UPDATERTYPE_NAME, _TEST_STATUS);
            Assert.That(_statusService?.GetStatusFor(_TEST_PROVIDER_NAME, _TEST_UPDATERTYPE_NAME)?.Status, Is.EqualTo(_TEST_STATUS.ToString()));
            Assert.Pass();
        }

        [Test]
        public void IComparableTest()
        {
            _statusService?.SetStatusFor(_TEST_PROVIDER_NAME, _TEST_UPDATERTYPE_NAME, _TEST_STATUS);
            Assert.IsTrue(_statusService?.GetStatusFor(_TEST_PROVIDER_NAME, _TEST_UPDATERTYPE_NAME) == _TEST_STATUS);
            Assert.Pass();
        }
    }
}