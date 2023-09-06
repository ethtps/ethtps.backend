using ETHTPS.API.Core.Integrations.MSSQL.Controllers;
using ETHTPS.API.Core.Integrations.MSSQL.Services.ApplicationData;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ETHTPS.API.Controllers.DataControllers
{
    [Route("/api/v3/external-website-categories")]
    [Authorize]
    [ApiController]
    public sealed class ExternalWebsiteCategoriesController : CRUDServiceControllerBase<ExternalWebsiteCategory>
    {
        public ExternalWebsiteCategoriesController(ExternalWebsiteCategoryService serviceImplementation) : base(serviceImplementation)
        {
        }
    }
}
