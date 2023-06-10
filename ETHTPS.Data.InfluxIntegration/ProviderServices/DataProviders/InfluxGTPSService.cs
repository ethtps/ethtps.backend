using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GTPS;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;

namespace ETHTPS.Data.Integrations.InfluxIntegration.ProviderServices.DataProviders
{
    public sealed class InfluxGTPSService : InfluxPSServiceBase, IGTPSService
    {
        public InfluxGTPSService(IInfluxWrapper influxWrapper, IRedisCacheService redisCacheService) : base(influxWrapper, x => x.GasUsed / Constants.GasPerTransfer, x => x.GPS / Constants.GasPerTransfer, redisCacheService)
        {
        }

        public async Task<List<DataResponseModel>> GetGTPSAsync(ProviderQueryModel requestModel, TimeInterval interval) => (await GetAsync(requestModel, interval)).SelectMany(x => x.Value).ToList();
    }
}
