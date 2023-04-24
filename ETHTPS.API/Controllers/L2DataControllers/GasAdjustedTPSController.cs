using System.Collections.Generic;

using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GTPS;
using ETHTPS.API.Core.Attributes;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers.L2DataControllers
{
    [Route("api/v2/GasAdjustedTPS/[action]")]
    public class GasAdjustedTPSController : IPSService
    {
        private readonly IGTPSService _gasAdjustedTPSService;

        public GasAdjustedTPSController(IGTPSService gasAdjustedTPSService)
        {
            _gasAdjustedTPSService = gasAdjustedTPSService;
        }

        [HttpGet]
        [TTL(3600)]
        public IDictionary<string, IEnumerable<DataResponseModel>> GetMonthlyDataByYear([FromQuery] ProviderQueryModel model, int year)
        {
            return _gasAdjustedTPSService.GetMonthlyDataByYear(model, year);
        }
        [HttpGet]
        [TTL(10)]
        public IDictionary<string, IEnumerable<DataResponseModel>> Get([FromQuery] ProviderQueryModel model, TimeInterval interval)
        {
            return _gasAdjustedTPSService.Get(model, interval);
        }
        [HttpGet]
        [TTL(1)]
        public IDictionary<string, IEnumerable<DataPoint>> Instant([FromQuery] ProviderQueryModel model)
        {
            return _gasAdjustedTPSService.Instant(model);
        }
        [HttpGet]
        [TTL(30)]
        public IDictionary<string, DataPoint> Max([FromQuery] ProviderQueryModel model)
        {
            return _gasAdjustedTPSService.Max(model);
        }
    }
}
