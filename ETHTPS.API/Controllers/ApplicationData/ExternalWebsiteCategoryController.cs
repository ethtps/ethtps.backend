using ETHTPS.API.Core.Integrations.MSSQL.Controllers;
using ETHTPS.API.Core.Integrations.MSSQL.Services.ApplicationData;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers.ApplicationData
{
    [Route("api/v3/data/external-website-category")]
    public sealed class ExternalWebsiteCategoryController : CRUDServiceControllerBase<ExternalWebsiteCategory>
    {
        public ExternalWebsiteCategoryController(ExternalWebsiteCategoryService serviceImplementation) : base(serviceImplementation)
        {
        }
    }
}
