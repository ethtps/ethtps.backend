using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GTPS;
using ETHTPS.Core;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Integrations.MSSQL;

namespace ETHTPS.Data.Integrations.InfluxIntegration.ProviderServices.DataProviders
{
    public sealed class InfluxGTPSService : InfluxPSServiceBase, IGTPSService
    {
        public InfluxGTPSService(IInfluxWrapper influxWrapper, IRedisCacheService redisCacheService, EthtpsContext context) : base(influxWrapper, x => x.GasUsed / Constants.GasPerTransfer, x => x.GPS / Constants.GasPerTransfer, redisCacheService, context)
        {
        }

        public async Task<List<DataResponseModel>> GetGTPSAsync(ProviderQueryModel requestModel, TimeInterval interval) => (await GetAsync(requestModel, interval)).SelectMany(x => x.Value).ToList();
    }
}
