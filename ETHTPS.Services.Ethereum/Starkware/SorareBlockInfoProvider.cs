using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Services.Attributes;
using ETHTPS.Services.Ethereum.Starkware.API.Models;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum.Starkware
{
    [Provider("Sorare")]
    [RunsEvery(CronConstants.EVERY_30_S)]
    public class SorareBlockInfoProvider : StarkwareBlockInfoProviderBase
    {
        public SorareBlockInfoProvider(EthtpsContext context, IConfiguration configuration) : base(Products.Sorare, context, configuration)
        {
        }
    }
}
