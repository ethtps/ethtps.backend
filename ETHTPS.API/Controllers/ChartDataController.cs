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
        public StreamchartModel GetStreamchartData([FromQuery] ChartDataRequestModel model)
        {
            return _chartDataServiceservice.GetStreamchartData(model);
        }

        [HttpGet]
        [TTL(5)]
        public StackedChartModel GetStackedChartData([FromQuery] ChartDataRequestModel model)
        {
            return _chartDataServiceservice.GetStackedChartData(model);
        }
    }
}
