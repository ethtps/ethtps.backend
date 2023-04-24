﻿using ETHTPS.Services.Attributes;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Polygon")]
    public class PolygonBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public PolygonBlockInfoProvider(IConfiguration configuration) : base(configuration, "PolygonEndpoint")
        {

        }
    }
}
