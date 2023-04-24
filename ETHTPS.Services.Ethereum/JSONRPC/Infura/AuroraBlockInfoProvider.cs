﻿using ETHTPS.Services.Attributes;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Aurora")]
    [RunsEvery(CronConstants.EVERY_30_S)]
    public sealed class AuroraBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public AuroraBlockInfoProvider(IConfiguration configuration) : base(configuration, "AuroraEndpoint")
        {

        }
    }
}
