using ETHTPS.Data.Core.Models.DataPoints;

namespace ETHTPS.API.BIL.Infrastructure.Services.DataServices
{
    public interface IPSService : IPSDataProvider<DataPoint, DataResponseModel>
    {
    }
}