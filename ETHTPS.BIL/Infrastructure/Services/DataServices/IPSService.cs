using ETHTPS.Data.Core.Models.DataPoints;

namespace ETHTPS.API.BIL.Infrastructure.Services.DataServices
{
    /// <summary>
    /// Represents a service that provides custom data points.
    /// </summary>
    /// <seealso cref="ETHTPS.API.BIL.Infrastructure.Services.DataServices.IPSDataProvider&lt;ETHTPS.Data.Core.Models.DataPoints.DataPoint, ETHTPS.Data.Core.Models.DataPoints.DataResponseModel&gt;" />
    public interface IPSService : IPSDataProvider<DataPoint, DataResponseModel>
    {
    }
}