using ETHTPS.API.Core.Integrations.MSSQL.Controllers;
using ETHTPS.API.Core.Integrations.MSSQL.Services.ApplicationData;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ETHTPS.API.Controllers.DataControllers
{
    [Route("/api/v3/external-websites")]
    [Authorize]
    [ApiController]
    public sealed class ExternalWebsitesController : CRUDServiceControllerBase<ExternalWebsite>
    {
        public ExternalWebsitesController(ExternalWebsitesService serviceImplementation) : base(serviceImplementation)
        {
        }
    }
}
