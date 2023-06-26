using System.Linq;

using ETHTPS.API.Core.Integrations.MSSQL.Controllers;
using ETHTPS.API.Core.Integrations.MSSQL.Services;
using ETHTPS.API.Core.Integrations.MSSQL.Services.ApplicationData;
using ETHTPS.Data.Core.Extensions;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ServiceStack;

using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ETHTPS.API.Controllers.DataControllers
{
    [Route("/api/v3/provider-links")]
    [Authorize]
    [ApiController]
    public sealed class ProviderLinksController : CRUDServiceControllerBase<ProviderLink>
    {
        private readonly GeneralService _generalService;
        public ProviderLinksController(ProviderLinksService serviceImplementation, GeneralService generalService) : base(serviceImplementation)
        {
            _generalService = generalService;
        }

        [HttpGet("GetLinksFor")]
        public IActionResult GetLinksFor(string providerName)
        {
            if (_generalService.AllProviders.Any(x => x.Name.EqualsIgnoreCase(providerName)))
            {
                var providerID = _generalService.AllProviders.First(x => x.Name.EqualsIgnoreCase(providerName)).Id;
                return Ok(base.GetAll().SafeWhere(x => x.ProviderId == providerID));
            }
            return BadRequest("Provider name invalid");
        }
    }
}
