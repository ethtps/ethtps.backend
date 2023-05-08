using ETHTPS.Configuration;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Services.Attributes;
using ETHTPS.Services.Ethereum.Starkware.API.Models;

namespace ETHTPS.Services.Ethereum.Starkware
{
    [Provider("DeversiFi")]
    [RunsEvery(CronConstants.EVERY_30_S)]
    public sealed class DeversiFIHTTPBlockInfoProvider : StarkwareBlockInfoProviderBase
    {
        public DeversiFIHTTPBlockInfoProvider(EthtpsContext context, IDBConfigurationProvider configuration) : base(Products.DeversiFi, context, configuration)
        {
        }
    }
}
