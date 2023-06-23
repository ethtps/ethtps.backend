using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Data.Core.Models.DataUpdater;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Services.BlockchainServices.HangfireLogging;
using ETHTPS.Services.Ethereum;
using ETHTPS.Services.LiveData;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ETHTPS.Tests.ProviderTests
{
    [Category("DataProviders")]
    [Category("TPS")]
    [Category("GPS")]
    [Category("GTPS")]
    [Category("Data")]
    [Category("HistoricalData")]
    [Category("Optional")]
    public abstract class ProviderTestBase<T> : TestBase
        where T : BlockInfoProviderBase
    {
        protected T? _provider;
        protected InfluxLogger<T>? _blockInfoLogger;
        protected IDataUpdaterStatusService? _statusService;
        protected WSAPIPublisher? _wsapiClient;

        [SetUp]
        public abstract void SetUp();

        protected void PartialSetup(T provider)
        {

            var context = ServiceProvider.GetRequiredService<EthtpsContext>();
            ILogger<HangfireBackgroundService> logger = ServiceProvider.GetRequiredService<ILogger<HangfireBackgroundService>>();
            _wsapiClient = ServiceProvider.GetRequiredService<WSAPIPublisher>();
            _statusService = ServiceProvider.GetRequiredService<IDataUpdaterStatusService>();
        }

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
                try
                {
                    result = await _provider.GetBlockInfoAsync((await _provider.GetLatestBlockInfoAsync()).Date);
                    Assert.NotNull(result);
                }
                catch (NotImplementedException)
                {
                    //("Overriding not implemented for this method because it is not evential");
                }
            });
        }

        [Test]
        public void LoggerNotNull_Test()
        {
            Assert.That(_blockInfoLogger, Is.Not.Null);
        }

        [Test]
        public void StatusService_NotNull_Test()
        {
            Assert.That(_statusService, Is.Not.Null);
        }

        [Test]
        public void LoggerRunAsync_ResultOk_Test()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                // Running this first because RunAsync swallows exceptions 
                await _blockInfoLogger.RunAsync();
                var s = _statusService.GetStatusFor(_provider.GetProviderName(), UpdaterType.TPSGPS)?.Status;

                if (s != null)
                    Assert.That(Enum.Parse<UpdaterStatus>(s), Is.EqualTo(UpdaterStatus.RanSuccessfully));
            });
        }
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.