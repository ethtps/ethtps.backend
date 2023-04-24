using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;

namespace ETHTPS.API.BIL.Infrastructure.Services.DataServices.GPS
{
    public interface IGPSProvider
    {
        List<DataResponseModel> GetGPS(ProviderQueryModel requestModel, TimeInterval interval);
    }
}
