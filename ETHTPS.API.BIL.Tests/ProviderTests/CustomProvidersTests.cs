using ETHTPS.Services.Ethereum;
using ETHTPS.Services.Ethereum.JSONRPC.Infura;

/// <summary>
/// Tests for custom implementations
/// </summary>
namespace ETHTPS.Tests.ProviderTests
{
    public sealed class ArbitrumNovaTests : ProviderTestBase<ArbitrumNovaBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ArbitrumNovaBlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class AVAXTests : ProviderTestBase<AVAXBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new AVAXBlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class AztecTests : ProviderTestBase<AztecBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new AztecBlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class BobaNetworkTests : ProviderTestBase<BobaNetworkBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new BobaNetworkBlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class HabitatTests : ProviderTestBase<HabitatBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new HabitatBlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class ImmutableXTests : ProviderTestBase<ImmutableXBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ImmutableXBlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class LoopringTests : ProviderTestBase<LoopringBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new LoopringBlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class MetisTests : ProviderTestBase<MetisBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new MetisBlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class Nahmii20Tests : ProviderTestBase<Nahmii20BlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new Nahmii20BlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class OMGNetworkTests : ProviderTestBase<OMGNetworkBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new OMGNetworkBlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class PolygonHermezTests : ProviderTestBase<PolygonHermezBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new PolygonHermezBlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class RoninTests : ProviderTestBase<RoninBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new RoninBlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class XDAITests : ProviderTestBase<XDAIHTTPBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new XDAIHTTPBlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class ZKSpaceTests : ProviderTestBase<ZKSpaceBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ZKSpaceBlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class ZKSyncTests : ProviderTestBase<ZKSsyncBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ZKSsyncBlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class ZKSwapTests : ProviderTestBase<ZKSwapBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ZKSwapBlockInfoProvider(ConfigurationProvider);
        }
    }

    public sealed class ZKTubeTests : ProviderTestBase<ZKTubeBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ZKTubeBlockInfoProvider(ConfigurationProvider);
        }
    }
}
