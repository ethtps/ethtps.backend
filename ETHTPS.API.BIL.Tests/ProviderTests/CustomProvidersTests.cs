using ETHTPS.Services.Ethereum;
using ETHTPS.Services.Ethereum.JSONRPC.Infura;

/// <summary>
/// Tests for custom implementations
/// </summary>
namespace ETHTPS.Tests.ProviderTests
{
    public class ArbitrumNovaTests : ProviderTestBase<ArbitrumNovaBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ArbitrumNovaBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class AVAXTests : ProviderTestBase<AVAXBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new AVAXBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class AztecTests : ProviderTestBase<AztecBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new AztecBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class BobaNetworkTests : ProviderTestBase<BobaNetworkBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new BobaNetworkBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class HabitatTests : ProviderTestBase<HabitatBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new HabitatBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class ImmutableXTests : ProviderTestBase<ImmutableXBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ImmutableXBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class LoopringTests : ProviderTestBase<LoopringBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new LoopringBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class MetisTests : ProviderTestBase<MetisBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new MetisBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class Nahmii20Tests : ProviderTestBase<Nahmii20BlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new Nahmii20BlockInfoProvider(ConfigurationProvider);
        }
    }

    public class OMGNetworkTests : ProviderTestBase<OMGNetworkBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new OMGNetworkBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class PolygonHermezTests : ProviderTestBase<PolygonHermezBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new PolygonHermezBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class RoninTests : ProviderTestBase<RoninBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new RoninBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class XDAITests : ProviderTestBase<XDAIHTTPBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new XDAIHTTPBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class ZKSpaceTests : ProviderTestBase<ZKSpaceBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ZKSpaceBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class ZKSyncTests : ProviderTestBase<ZKSsyncBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ZKSsyncBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class ZKSwapTests : ProviderTestBase<ZKSwapBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ZKSwapBlockInfoProvider(ConfigurationProvider);
        }
    }

    public class ZKTubeTests : ProviderTestBase<ZKTubeBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ZKTubeBlockInfoProvider(ConfigurationProvider);
        }
    }
}
