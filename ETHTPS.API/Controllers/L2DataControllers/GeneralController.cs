using System.Collections.Generic;
using System.Threading.Tasks;

using ETHTPS.API.Core.Attributes;
using ETHTPS.API.Core.Integrations.MSSQL.Services;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.ResponseModels;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers.L2DataControllers
{
    [Route("api/v2/[action]")]
    public class GeneralController
    {
        private readonly GeneralService _generalService;

        public GeneralController(GeneralService generalService)
        {
            _generalService = generalService;
        }

        [HttpGet]
        [TTL(3600)]
        public IEnumerable<string> Networks()
        {
            return _generalService.Networks();
        }

        [HttpGet]
        [TTL(3600)]
        public IEnumerable<TimeInterval> Intervals()
        {
            return _generalService.Intervals();
        }


        [HttpGet]
        [TTL(3600)]
        public IEnumerable<ProviderResponseModel> Providers(string subchainsOf)
        {
            if (!string.IsNullOrWhiteSpace(subchainsOf))
                return _generalService.Providers(subchainsOf);

            return _generalService.AllProviders;
        }

        [HttpGet]
        [TTL(3600)]
        public IDictionary<string, string> ColorDictionary()
        {
            return _generalService.ColorDictionary();
        }

        [HttpGet]
        [TTL(3600)]
        public IDictionary<string, string> ProviderTypesColorDictionary()
        {
            return _generalService.ProviderTypesColorDictionary();
        }

        [HttpGet]
        [TTL(1)]
        public async Task<IDictionary<string, object>> InstantDataAsync([FromQuery] ProviderQueryModel model, string smoothing = "")
        {
            return await _generalService.InstantDataAsync(model, smoothing);
        }

        [HttpGet]
        [TTL(30)]
        public async Task<IDictionary<string, object>> MaxAsync([FromQuery] ProviderQueryModel model)
        {
            return await _generalService.MaxAsync(model);
        }   /// <summary>
            /// Used for displaying chart buttons.
            /// </summary>
        [HttpGet]
        [TTL(60)]
        public async Task<IEnumerable<TimeInterval>> GetIntervalsWithDataAsync([FromQuery] ProviderQueryModel model)
        {
            return await _generalService.GetIntervalsWithDataAsync(model);
        }

        [HttpGet]
        [TTL(60)]
        public async Task<IEnumerable<string>> GetUniqueDataYearsAsync([FromQuery] ProviderQueryModel model)
        {
            return await _generalService.GetUniqueDataYearsAsync(model);
        }

        [HttpGet]
        [TTL(30)]
        public async Task<AllDataModel> AllDataAsync(string network = "Mainnet")
        {
            return await _generalService.GetAllDataAsync(network);
        }
    }
}