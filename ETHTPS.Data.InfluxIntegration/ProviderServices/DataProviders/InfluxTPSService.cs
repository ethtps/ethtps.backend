using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.TPS;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;

namespace ETHTPS.Data.Integrations.InfluxIntegration.ProviderServices.DataProviders
{
    public sealed class InfluxTPSService : InfluxPSServiceBase, ITPSService
    {
        public InfluxTPSService(IInfluxWrapper influxWrapper, IRedisCacheService redisCacheService) : base(influxWrapper, x => x.TransactionCount, redisCacheService)
        {
        }

        public async Task<List<DataResponseModel>> GetTPSAsync(ProviderQueryModel requestModel, TimeInterval interval) => (await GetAsync(requestModel, interval)).SelectMany(x => x.Value).ToList();
    }
}
