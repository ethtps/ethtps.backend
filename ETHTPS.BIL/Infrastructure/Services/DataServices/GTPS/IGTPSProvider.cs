using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;

namespace ETHTPS.API.BIL.Infrastructure.Services.DataServices.GTPS
{
    public interface IGTPSProvider
    {
        Task<List<DataResponseModel>> GetGTPSAsync(ProviderQueryModel requestModel, TimeInterval interval);
    }
}
