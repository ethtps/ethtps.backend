﻿using ETHTPS.Services.Ethereum;
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
            PartialSetup(_provider);
        }
    }

    public sealed class AVAXTests : ProviderTestBase<AVAXBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new AVAXBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class AztecTests : ProviderTestBase<AztecBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new AztecBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class BobaNetworkTests : ProviderTestBase<BobaNetworkBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new BobaNetworkBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class HabitatTests : ProviderTestBase<HabitatBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new HabitatBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class ImmutableXTests : ProviderTestBase<ImmutableXBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ImmutableXBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class LoopringTests : ProviderTestBase<LoopringBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new LoopringBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class MetisTests : ProviderTestBase<MetisBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new MetisBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class Nahmii20Tests : ProviderTestBase<Nahmii20BlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new Nahmii20BlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }
    /*
    public sealed class OMGNetworkTests : ProviderTestBase<OMGNetworkBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new OMGNetworkBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }
    */
    public sealed class PolygonHermezTests : ProviderTestBase<PolygonHermezBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new PolygonHermezBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class RoninTests : ProviderTestBase<RoninBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new RoninBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class XDAITests : ProviderTestBase<XDAIHTTPBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new XDAIHTTPBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class ZKSpaceTests : ProviderTestBase<ZKSpaceBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ZKSpaceBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class ZKSyncTests : ProviderTestBase<ZKSsyncBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ZKSsyncBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class ZKSwapTests : ProviderTestBase<ZKSwapBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ZKSwapBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }

    public sealed class ZKTubeTests : ProviderTestBase<ZKTubeBlockInfoProvider>
    {
        [SetUp]
        public override void SetUp()
        {
            _provider = new ZKTubeBlockInfoProvider(ConfigurationProvider);
            PartialSetup(_provider);
        }
    }
}