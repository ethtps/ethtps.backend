﻿using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Aurora")]
    [RunsEvery(CronConstants.EVERY_30_S)]
    public sealed class AuroraBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public AuroraBlockInfoProvider(DBConfigurationProviderWithCache configurationProvider) : base(configurationProvider, "Aurora")
        {
        }
    }
}
