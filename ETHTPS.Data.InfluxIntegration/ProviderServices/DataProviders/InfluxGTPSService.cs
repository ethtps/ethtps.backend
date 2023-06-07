using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GTPS;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;

namespace ETHTPS.Data.Integrations.InfluxIntegration.ProviderServices.DataProviders
{
    public sealed class InfluxGTPSService : IGTPSService
    {
        public Task<IDictionary<string, IEnumerable<DataResponseModel>>> GetAsync(ProviderQueryModel model, TimeInterval interval)
        {
            throw new NotImplementedException();
        }

        public Task<List<DataResponseModel>> GetGTPSAsync(ProviderQueryModel requestModel, TimeInterval interval)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, IEnumerable<DataResponseModel>>> GetMonthlyDataByYearAsync(ProviderQueryModel model, int year)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, IEnumerable<DataPoint>>> InstantAsync(ProviderQueryModel model)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, DataPoint>> MaxAsync(ProviderQueryModel model)
        {
            throw new NotImplementedException();
        }
    }
}
