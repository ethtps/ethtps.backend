using System.Linq;
using System.Threading.Tasks;

using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.API.Core.Controllers;
using ETHTPS.API.Core.Integrations.MSSQL.Services;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Core.Models.ResponseModels.L2s;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace ETHTPS.API.Controllers.L2DataControllers
{
    [Route("/api/v3/L2Data/[action]")]
    public sealed class L2DataController : APIControllerBase
    {
        private readonly IAggregatedDataService _aggregatedDataService;
        private readonly IPSDataFormatter _dataFormatter;
        private readonly GeneralService _generalService;

        public L2DataController(IAggregatedDataService aggregatedDataService, IPSDataFormatter dataFormatter, GeneralService generalService)
        {
            _aggregatedDataService = aggregatedDataService;
            _dataFormatter = dataFormatter;
            _generalService = generalService;
        }

        /// <summary>
        /// Provides a catch-all endpoint for data requests. There are many ways requests can be customized; invalid parameters will
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(200, Type = typeof(L2DataResponseModel))]
        [SwaggerResponse(400, "Invalid parameter(s)", Type = typeof(ValidationResult))]
        public async Task<IActionResult> GetAsync([FromBody] L2DataRequestModel requestModel, DataType dataType)
        {
            var providers = _generalService.AllProviders.Select(x => (x.Name, x.Type == "Sidechain")).Where(x => !requestModel.IncludeSidechains ? !x.Item2 : true);
            if (requestModel.Providers != null)
                if (requestModel.Providers.Contains(Constants.All))
                    requestModel.Providers = providers.Select(x => x.Name).ToList();
                else requestModel.Providers = requestModel.Providers.Where(p => providers.Select(x => x.Name).Contains(p)).ToList();
            var validationResult = requestModel.Validate(providers.Select(x => x.Name));
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Reason);
            }
            return Ok(await _aggregatedDataService.GetDataAsync(requestModel, dataType, _dataFormatter));
        }
    }
}