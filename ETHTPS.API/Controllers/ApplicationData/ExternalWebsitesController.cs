using System.Collections.Generic;

using ETHTPS.API.Core.Attributes;
using ETHTPS.API.Core.Integrations.MSSQL.Controllers;
using ETHTPS.API.Core.Integrations.MSSQL.Services.ApplicationData;
using ETHTPS.Data.Core.Models.ExternalWebsites;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers.ApplicationData
{
    [Route("/api/v3/data/external-websites/")]
    [ApiController]
    public sealed class ExternalWebsitesController : CRUDServiceControllerBase<ExternalWebsite>
    {
        private readonly ExternalWebsitesService _externalWebsitesService;
        public ExternalWebsitesController(ExternalWebsitesService serviceImplementation) : base(serviceImplementation)
        {
            _externalWebsitesService = serviceImplementation;
        }

        [HttpGet]
        [TTL(60)]
        public IEnumerable<IProviderExternalWebsite> GetExternalWebsitesFor(string providerName)
        {
            return _externalWebsitesService.GetExternalWebsitesFor(providerName);
        }
    }
}
