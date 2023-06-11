using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;

namespace ETHTPS.API.BIL.Infrastructure.Services.DataServices
{
    /// <summary>
    /// Represents a (p)er-(s)econd controller. This interface is used for making sure both TPS and GPS controller methods always return the same types.
    /// </summary>
    /// <typeparam name="TDataPoint"></typeparam>
    /// <typeparam name="TResponseModel"></typeparam>
    public interface IPSDataProvider<TDataPoint, TResponseModel>
    {
        Task<IDictionary<string, IEnumerable<TResponseModel>>> GetAsync(L2DataRequestModel model);
        Task<IDictionary<string, IEnumerable<TResponseModel>>> GetAsync(ProviderQueryModel model, TimeInterval interval);
        Task<IDictionary<string, TDataPoint>> MaxAsync(ProviderQueryModel model);
        Task<IDictionary<string, IEnumerable<TDataPoint>>> InstantAsync(ProviderQueryModel model);
        Task<IDictionary<string, IEnumerable<TResponseModel>>> GetMonthlyDataByYearAsync(ProviderQueryModel model, int year);
    }
}
