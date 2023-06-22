using ETHTPS.API.Core.Integrations.MSSQL.Controllers;
using ETHTPS.API.Core.Integrations.MSSQL.Services.ApplicationData;
using ETHTPS.Configuration.Database;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers.ApplicationData
{
    [Route("api/v3/data/microservices")]
    public sealed class MicroservicesController : CRUDServiceControllerBase<Microservice>
    {
        public MicroservicesController(MicroservicesService serviceImplementation) : base(serviceImplementation)
        {
        }
    }
}
