using ETHTPS.API.Core.Integrations.MSSQL.Controllers;
using ETHTPS.API.Core.Integrations.MSSQL.Services.ApplicationData;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers.ApplicationData
{
    [Route("api/v3/data/provider-links")]
    public sealed class ProviderLinksController : CRUDServiceControllerBase<ProviderLink>
    {
        public ProviderLinksController(ProviderLinksService serviceImplementation) : base(serviceImplementation)
        {
        }
    }
}
