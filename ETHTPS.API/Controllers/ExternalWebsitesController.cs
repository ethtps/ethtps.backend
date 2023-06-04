using System.Collections.Generic;

using ETHTPS.API.BIL.Infrastructure.Services;
using ETHTPS.API.Core.Attributes;
using ETHTPS.API.Core.Integrations.MSSQL.Controllers;
using ETHTPS.Data.Core.Models.ExternalWebsites;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers
{
    [Route("/api/v3/external-websites/")]
    [ApiController]
    public sealed class ExternalWebsitesController : CRUDServiceControllerBase<IExternalWebsite>
    {
        private readonly IExternalWebsitesService _externalWebsitesService;
        public ExternalWebsitesController(IExternalWebsitesService serviceImplementation) : base((ICRUDService<IExternalWebsite>)serviceImplementation)
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
