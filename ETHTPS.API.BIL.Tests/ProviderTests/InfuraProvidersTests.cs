using ETHTPS.Services.BlockchainServices.BlockTime;
using ETHTPS.Services.Ethereum.JSONRPC.Infura;

using Microsoft.Extensions.DependencyInjection;

// Putting all of these in a single file
namespace ETHTPS.Tests.ProviderTests
{
    public sealed class EthereumTests : ProviderTestBase<EthereumBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new EthereumBlockInfoProvider(ConfigurationProvider, ServiceProvider.GetRequiredService<EthereumBlockTimeProvider>());
            PartialSetup(_provider);
        }
    }

    public sealed class PalmTests : ProviderTestBase<PalmBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new PalmBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class OptimismTests : ProviderTestBase<OptimismBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new OptimismBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class NEARTests : ProviderTestBase<NEARBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new NEARBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class StarknetTests : ProviderTestBase<StarknetBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new StarknetBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class AuroraTests : ProviderTestBase<AuroraBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new AuroraBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class CeloTests : ProviderTestBase<CeloBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new CeloBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class PolygonTests : ProviderTestBase<PolygonBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new PolygonBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class ArbitrumTests : ProviderTestBase<ArbitrumBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ArbitrumBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }
}
