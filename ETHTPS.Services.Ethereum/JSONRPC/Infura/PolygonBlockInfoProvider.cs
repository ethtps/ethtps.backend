﻿using ETHTPS.Configuration;
using ETHTPS.Services.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Polygon")]
    [RunsEvery(CronConstants.EVERY_5_S)]
    public sealed class PolygonBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public PolygonBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Polygon")
        {
        }
    }
}
