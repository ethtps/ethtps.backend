using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GPS;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Integrations.MSSQL;

namespace ETHTPS.Data.Integrations.InfluxIntegration.ProviderServices.DataProviders
{
    public sealed class InfluxGPSService : InfluxPSServiceBase, IGPSService
    {
        public InfluxGPSService(IInfluxWrapper influxWrapper, IRedisCacheService redisCacheService, EthtpsContext context) : base(influxWrapper, x => x.GasUsed, x => x.GPS, redisCacheService, context)
        {
        }

        public async Task<List<DataResponseModel>> GetGPSAsync(ProviderQueryModel requestModel, TimeInterval interval) => (await GetAsync(requestModel, interval)).SelectMany(x => x.Value).ToList();
    }
}
