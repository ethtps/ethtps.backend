using System.Linq;
using System.Threading.Tasks;

using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GPS;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GTPS;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.TPS;
using ETHTPS.API.Core.Attributes;
using ETHTPS.API.Core.Integrations.MSSQL.Services;
using ETHTPS.API.Core.Integrations.MSSQL.Services.ApplicationData;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Extensions.StringExtensions;
using ETHTPS.Data.Core.Models.Pages.Chart;
using ETHTPS.Data.Core.Models.Pages.HomePage;
using ETHTPS.Data.Core.Models.Pages.ProviderPage;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Core.Models.ResponseModels.SocialMedia;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers
{
    [Route("api/v2/Pages/[action]")]
    [Authorize]
    [ApiController]
    public sealed class PageModelController : ControllerBase
    {
        private readonly GeneralService _generalService;
        private readonly IGTPSService _gasAdjustedTPSService;
        private readonly ITPSService _tpsService;
        private readonly IGPSService _gpsService;
        private readonly ExternalWebsitesService _externalWebsitesService;
        private readonly ExternalWebsiteCategoryService _externalWebsiteCategoryService;

        public PageModelController(GeneralService generalService, IGTPSService gasAdjustedTPSService, ITPSService tpsService, IGPSService gpsService, ExternalWebsitesService externalWebsitesService, ExternalWebsiteCategoryService externalWebsiteCategoryService)
        {
            _generalService = generalService;
            _gasAdjustedTPSService = gasAdjustedTPSService;
            _tpsService = tpsService;
            _gpsService = gpsService;
            _externalWebsitesService = externalWebsitesService;
            _externalWebsiteCategoryService = externalWebsiteCategoryService;
        }

        [HttpGet]
        [TTL(10)]
        public async Task<HomePageResponseModel> HomeAsync([FromQuery] HomePageRequestModel model) => new()
        {
            ChartData = await FromRequestModelAsync(model),
            MaxData = await _generalService.MaxAsync(ProviderQueryModel.All),
            InstantData = await _generalService.InstantDataAsync(ProviderQueryModel.All),
            ColorDictionary = _generalService.ColorDictionary(),
            ProviderTypesColorDictionary = _generalService.ProviderTypesColorDictionary(),
            Providers = _generalService.Providers(model.SubchainsOf)
        };

        [HttpGet]
        [TTL(10)]
        public async Task<IActionResult> ProviderAsync([FromQuery] ProviderPageRequestModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Provider) || model.Provider.LossyCompareTo(Constants.All) || !_generalService.AllProviders.Any(x => x.Name.LossyCompareTo(model.Provider)))
            {
                return BadRequest($"Invalid provider name \"{model.Provider}\"");
            }
            return Ok(new ProviderPageResponseModel()
            {
                ChartData = await FromRequestModelAsync(model),
                IntervalsWithData = await _generalService.GetIntervalsWithDataAsync(model),
                UniqueDataYears = await _generalService.GetUniqueDataYearsAsync(model),
                Links = new()
                {
                    CategorizedLinks = _externalWebsitesService
                        .GetExternalWebsitesFor(model.Provider)
                        .GroupBy(x => x.Category)
                        .ToDictionary(category => _externalWebsiteCategoryService.GetById(category.Key).Name,
                            group => group.Select(v => new ProviderSocialMediaLink()
                            {
                                Link = v.Url,
                                WebsiteName = v.Name
                            }))
                }
            });
        }

        private async Task<ChartData> FromRequestModelAsync(RequestModelWithChartBase model) => new ChartData()
        {
            Data = await GetServiceFor(model.DataType).GetAsync(model, model.Interval),
            DataType = model.DataType
        };

        private IPSService GetServiceFor(DataType dataType) => dataType switch
        {
            DataType.TPS => _tpsService,
            DataType.GPS => _gpsService,
            _ => _gasAdjustedTPSService,
        };
    }
}
