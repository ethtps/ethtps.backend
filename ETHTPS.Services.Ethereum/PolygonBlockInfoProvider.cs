﻿using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Services.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC
{
    [Provider("Polygon")]
    public sealed class PolygonBlockInfoProvider : JSONRPCBlockInfoProviderBase, IHTTPBlockInfoProvider
    {
        public PolygonBlockInfoProvider(string endpoint) : base(endpoint)
        {
        }
    }
}
