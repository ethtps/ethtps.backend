using ETHTPS.API.BIL.Infrastructure.Services;
using ETHTPS.API.Core.Integrations.MSSQL.Controllers;
using ETHTPS.API.Core.Integrations.MSSQL.Services.ApplicationData;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers.ApplicationData
{
    [Route("api/v3/data/providers")]
    public sealed class ProvidersController : CRUDServiceControllerBase<Provider>
    {
        public ProvidersController(ProvidersService serviceImplementation) : base((ICRUDService<Provider>)serviceImplementation)
        {
        }
    }
}
