using System.Threading.Tasks;

using ETHTPS.API.BIL.Infrastructure.Services.ChartData;
using ETHTPS.API.Core.Attributes;
using ETHTPS.API.Core.Controllers;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Core.Models.ResponseModels.ChartData.StackedChart;
using ETHTPS.Data.Core.Models.ResponseModels.ChartData.Streamchart;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers
{
    [Route("/api/v3/ChartData/[action]")]
    public sealed class ChartDataController : APIControllerBase
    {
        private readonly IChartDataServiceservice _chartDataServiceservice;

        public ChartDataController(IChartDataServiceservice chartDataServiceservice)
        {
            _chartDataServiceservice = chartDataServiceservice;
        }

        [HttpGet]
        [TTL(5)]
        public async Task<StreamchartModel> GetStreamchartDataAsync([FromQuery] ChartDataRequestModel model)
        {
            return await _chartDataServiceservice.GetStreamchartDataAsync(model);
        }

        [HttpGet]
        [TTL(5)]
        public async Task<StackedChartModel> GetStackedChartDataAsync([FromQuery] ChartDataRequestModel model)
        {
            return await _chartDataServiceservice.GetStackedChartDataAsync(model);
        }
    }
}
