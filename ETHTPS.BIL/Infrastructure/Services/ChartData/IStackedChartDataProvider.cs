using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Core.Models.ResponseModels.ChartData.StackedChart;

namespace ETHTPS.API.BIL.Infrastructure.Services.ChartData
{
    public interface IStackedChartDataProvider
    {
        Task<StackedChartModel> GetStackedChartDataAsync(ChartDataRequestModel model);
    }
}
