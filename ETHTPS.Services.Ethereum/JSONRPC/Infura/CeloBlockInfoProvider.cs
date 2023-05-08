﻿using ETHTPS.Configuration;
using ETHTPS.Services.Attributes;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    [Provider("Celo")]
    [RunsEvery(CronConstants.EVERY_30_S)]
    public sealed class CeloBlockInfoProvider : InfuraBlockInfoProviderBase
    {
        public CeloBlockInfoProvider(IDBConfigurationProvider configurationProvider) : base(configurationProvider, "Celo")
        {
        }
    }
}
