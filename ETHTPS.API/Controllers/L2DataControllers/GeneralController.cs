using System.Collections.Generic;

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
        public IDictionary<string, object> InstantData([FromQuery] ProviderQueryModel model, string smoothing = "")
        {
            return _generalService.InstantData(model, smoothing);
        }

        [HttpGet]
        [TTL(30)]
        public IDictionary<string, object> Max([FromQuery] ProviderQueryModel model)
        {
            return _generalService.Max(model);
        }   /// <summary>
            /// Used for displaying chart buttons.
            /// </summary>
        [HttpGet]
        [TTL(60)]
        public IEnumerable<TimeInterval> GetIntervalsWithData([FromQuery] ProviderQueryModel model)
        {
            return _generalService.GetIntervalsWithData(model);
        }

        [HttpGet]
        [TTL(60)]
        public IEnumerable<string> GetUniqueDataYears([FromQuery] ProviderQueryModel model)
        {
            return _generalService.GetUniqueDataYears(model);
        }

        [HttpGet]
        [TTL(30)]
        public AllDataModel AllData(string network = "Mainnet")
        {
            return _generalService.GetAllData(network);
        }
    }
}