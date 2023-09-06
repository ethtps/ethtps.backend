using ETHTPS.Configuration;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Services.Ethereum.Starkware.API.Models;

namespace ETHTPS.Services.Ethereum.Starkware
{
    [Provider("Sorare")]
    [RunsEvery(CronConstants.EVERY_30_S)]
    public sealed class SorareBlockInfoProvider : StarkwareBlockInfoProviderBase
    {
        public SorareBlockInfoProvider(EthtpsContext context, DBConfigurationProviderWithCache configuration) : base(Products.Sorare, context, configuration)
        {
        }
    }
}
