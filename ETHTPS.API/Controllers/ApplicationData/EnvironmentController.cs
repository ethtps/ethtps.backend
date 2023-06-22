using ETHTPS.API.Core.Integrations.MSSQL.Controllers;
using ETHTPS.API.Core.Integrations.MSSQL.Services.ApplicationData;
using ETHTPS.Configuration.Database;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers.ApplicationData
{
    [Route("api/v3/data/environments")]
    public sealed class EnvironmentController : CRUDServiceControllerBase<Environment>
    {
        public EnvironmentController(EnvironmentService serviceImplementation) : base(serviceImplementation)
        {
        }
    }
}
