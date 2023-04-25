using System;
using System.Linq;
using System.Threading.Tasks;

using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.API.Core.Controllers;
using ETHTPS.API.Core.Integrations.MSSQL.Services;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Core.Models.ResponseModels.L2s;
using ETHTPS.Services.Infrastructure.Messaging;

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
        private readonly IRedisCacheService _redisCacheService;
        private readonly IMessagePublisher _messagePublisher;

        public L2DataController(IAggregatedDataService aggregatedDataService, IPSDataFormatter dataFormatter, GeneralService generalService, IRedisCacheService redisCacheService, IMessagePublisher messagePublisher)
        {
            _aggregatedDataService = aggregatedDataService;
            _dataFormatter = dataFormatter;
            _generalService = generalService;
            _redisCacheService = redisCacheService;
            _messagePublisher = messagePublisher;
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
            var validationResult = Validate(requestModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Reason);
            }
            return Ok(await _aggregatedDataService.GetDataAsync(requestModel, dataType, _dataFormatter));
        }

        [HttpGet]
        [SwaggerResponse(200, Type = typeof(L2DataResponseModel))]
        [SwaggerResponse(statusCode: 202)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> GetDataRequestAsync(string guid)
        {
            if (!await _redisCacheService.HasKeyAsync(guid))
            {
                if (await _redisCacheService.HasKeyAsync(L2DataRequestStatus.PREFIX + guid))
                {
                    var status = await _redisCacheService.GetDataAsync<L2DataRequestStatus>(L2DataRequestStatus.PREFIX + guid);
                    if (status.State != L2DataRequestState.Failed && status.State != L2DataRequestState.Completed)
                    {
                        return Accepted(status);
                    }
                }
                return NotFound();
            }
            var result = await _redisCacheService.GetDataAsync<L2DataResponseModel>(guid);
            return Ok();
        }

        [HttpGet]
        [SwaggerResponse(200, Type = typeof(L2DataResponseModel))]
        [SwaggerResponse(404)]
        public async Task<IActionResult> GetDataRequestStatusAsync(string guid)
        {
            if (!await _redisCacheService.HasKeyAsync(L2DataRequestStatus.PREFIX + guid))
            {
                return NotFound(guid);
            }
            return Ok(await _redisCacheService.GetDataAsync<L2DataRequestStatus>(L2DataRequestStatus.PREFIX + guid)); //Not implemented yet
        }

        /// <summary>
        /// More complex data requests can be created by posting to this endpoint. The request will be queued and processed asynchronously. The response will contain a GUID that can be used to retrieve the data and the user will be notified via SignalR about the status of the request and about when the data is ready.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(201)]
        [SwaggerResponse(400, "Invalid parameter(s)", Type = typeof(ValidationResult))]
        public async Task<IActionResult> CreateDataRequestAsync([FromBody] L2DataRequestModel requestModel, DataType dataType)
        {
            requestModel ??= new L2DataRequestModel()
            {
                Providers = new()
                    {
                        "Ethereum"
                    },
                StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                IncludeEmptyDatasets = true,
                IncludeSidechains = true,
                IncludeComplexAnalysis = true,
                IncludeSimpleAnalysis = true,
            };
            var validationResult = Validate(requestModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Reason);
            }
            var guid = Guid.NewGuid();
            await _redisCacheService.SetDataAsync(L2DataRequestStatus.PREFIX + guid.ToString(), new L2DataRequestStatus(), TimeSpan.FromHours(1));
            await _redisCacheService.SetDataAsync(L2DataRequestModel.PREFIX + guid.ToString(), requestModel, TimeSpan.FromMinutes(15));
            _messagePublisher.PublishJSONMessage(requestModel, "L2DataRequestQueue");
            return Created(nameof(GetDataRequestAsync), guid);
        }

        /// <summary>
        /// Checks if the request is valid and sets the default values for the request if they are not already set.
        /// </summary>
        private ValidationResult Validate(L2DataRequestModel requestModel)
        {
            if (requestModel == null)
                return new ValidationResult(false, "Request model is null");

            var providers = _generalService.AllProviders.Select(x => (x.Name, x.Type == "Sidechain")).Where(x => !requestModel.IncludeSidechains ? !x.Item2 : true);
            if (requestModel.Providers != null)
                if (requestModel.Providers.Contains(Constants.All))
                    requestModel.Providers = providers.Select(x => x.Name).ToList();
                else requestModel.Providers = requestModel.Providers.Where(p => providers.Select(x => x.Name).Contains(p)).ToList();
            return requestModel.Validate(providers.Select(x => x.Name));
        }
    }
}