using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Services.Attributes;
using ETHTPS.Services.BlockchainServices;
using ETHTPS.Services.Ethereum.Starkware.API.Models;

using Microsoft.Extensions.Configuration;

namespace ETHTPS.Services.Ethereum.Starkware
{
    [Provider("DeversiFi")]
    [RunsEvery(CronConstants.EVERY_30_S)]
    public class DeversiFIHTTPBlockInfoProvider : StarkwareBlockInfoProviderBase
    {
        public DeversiFIHTTPBlockInfoProvider(EthtpsContext context, IConfiguration configuration) : base(Products.DeversiFi, context, configuration)
        {
        }
    }
}
