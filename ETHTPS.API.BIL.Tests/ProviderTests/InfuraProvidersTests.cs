using ETHTPS.Services.BlockchainServices.BlockTime;
using ETHTPS.Services.Ethereum.JSONRPC.Infura;

using Microsoft.Extensions.DependencyInjection;

// Putting all of these in a single file
namespace ETHTPS.Tests.ProviderTests.InfuraProviderTests
{
    public class EthereumTests : ProviderTestBase<EthereumBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new EthereumBlockInfoProvider(ConfigurationProvider, ServiceProvider.GetRequiredService<EthereumBlockTimeProvider>());
        }
    }

    public class PalmTests : ProviderTestBase<PalmBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new PalmBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class OptimismTests : ProviderTestBase<OptimismBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new OptimismBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class NEARTests : ProviderTestBase<NEARBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new NEARBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class StarknetTests : ProviderTestBase<StarknetBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new StarknetBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class AuroraTests : ProviderTestBase<AuroraBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new AuroraBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class CeloTests : ProviderTestBase<CeloBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new CeloBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class PolygonTests : ProviderTestBase<PolygonBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new PolygonBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class ArbitrumTests : ProviderTestBase<ArbitrumBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ArbitrumBlockInfoProvider(ConfigurationProvider);
        }
    }
}
