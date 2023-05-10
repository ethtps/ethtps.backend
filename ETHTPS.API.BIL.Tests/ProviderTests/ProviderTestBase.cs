using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.Core.Integrations.MSSQL.Services.TimeBuckets;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Data.Core.Models.DataUpdater;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Services.BlockchainServices.HangfireLogging;
using ETHTPS.Services.Ethereum;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ETHTPS.Tests.ProviderTests
{
    public abstract class ProviderTestBase<T> : TestBase
        where T : BlockInfoProviderBase
    {
        protected T? _provider;
        protected MSSQLLogger<T>? _blockInfoLogger;
        protected IDataUpdaterStatusService? _statusService;

        [SetUp]
        public abstract void SetUp();

        protected void PartialSetup(T provider)
        {

            var context = ServiceProvider.GetRequiredService<EthtpsContext>();
            ILogger<HangfireBackgroundService> logger = ServiceProvider.GetRequiredService<ILogger<HangfireBackgroundService>>();
            ILogger<MSSQLTimeBucketService<T>> logger2 = ServiceProvider.GetRequiredService<ILogger<MSSQLTimeBucketService<T>>>();
            var timeBucketService = new MSSQLTimeBucketService<T>(provider, context, logger2);
            _statusService = ServiceProvider.GetRequiredService<IDataUpdaterStatusService>();
            _blockInfoLogger = new Services.BlockchainServices.HangfireLogging.MSSQLLogger<T>(provider, logger, context, _statusService, timeBucketService);
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
                result = await _provider.GetBlockInfoAsync((await _provider.GetLatestBlockInfoAsync()).Date);
                Assert.NotNull(result);
            });
        }

        [Test]
        public void LoggerNotNull_Test()
        {
            Assert.NotNull(_blockInfoLogger);
        }

        [Test]
        public void StatusService_NotNull_Test()
        {
            Assert.NotNull(_statusService);
        }

        [Test]
        public void LoggerRunAsync_NoException_Test()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                await _blockInfoLogger.RunWithoutExceptionHandlingAsync();
            });
        }

        [Test]
        public void LoggerRunAsync_ResultOk_Test()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                // Running this first because RunAsync swallows exceptions 
                await _blockInfoLogger.RunWithoutExceptionHandlingAsync();
                await _blockInfoLogger.RunAsync();
                var s = _statusService.GetStatusFor(_provider.GetProviderName(), UpdaterType.TPSGPS)?.Status;

                if (s != null)
                    Assert.That(Enum.Parse<UpdaterStatus>(s), Is.EqualTo(UpdaterStatus.RanSuccessfully));
            });
        }
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.