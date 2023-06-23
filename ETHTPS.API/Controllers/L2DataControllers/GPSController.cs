using System.Collections.Generic;
using System.Threading.Tasks;

using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GPS;
using ETHTPS.API.Core.Attributes;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers.L2DataControllers
{
    [Route("api/v2/GPS/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "APIKey")]
    public sealed class GPSController : ControllerBase, IPSService
    {
        private readonly IGPSService _gpsService;

        public GPSController(IGPSService tpsService)
        {
            _gpsService = tpsService;
        }

        [HttpGet]
        [TTL(3600)]
        public async Task<IDictionary<string, IEnumerable<DataResponseModel>>> GetMonthlyDataByYearAsync([FromQuery] ProviderQueryModel model, int year)
        {
            return await _gpsService.GetMonthlyDataByYearAsync(model, year);
        }

        [HttpGet]
        [TTL(10)]
        public async Task<IDictionary<string, IEnumerable<DataResponseModel>>> GetAsync([FromQuery] ProviderQueryModel model, TimeInterval interval)
        {
            return await _gpsService.GetAsync(model, interval);
        }

        [HttpGet]
        [TTL(1)]
        public async Task<IDictionary<string, IEnumerable<DataPoint>>> InstantAsync([FromQuery] ProviderQueryModel model)
        {
            return await _gpsService.InstantAsync(model);
        }

        [HttpGet]
        [TTL(30)]
        public async Task<IDictionary<string, DataPoint>> MaxAsync([FromQuery] ProviderQueryModel model)
        {
            return await _gpsService.MaxAsync(model);
        }

        [HttpPost]
        [TTL(10)]
        public async Task<IDictionary<string, IEnumerable<DataResponseModel>>> GetAsync([FromBody] L2DataRequestModel model)
        {
            return await _gpsService.GetAsync(model);
        }
    }
}
