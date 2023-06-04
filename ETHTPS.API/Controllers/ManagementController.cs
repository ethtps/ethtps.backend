
using System.Collections.Generic;

using ETHTPS.Configuration;
using ETHTPS.Data.Core.Models.Configuration;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers
{
    [Route("/api/v3/Experiments/[action]")]
    [ApiController]
    public sealed class ManagementController : ControllerBase
    {
        private readonly IDBConfigurationProvider _configurationProvider;

        public ManagementController(IDBConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        [HttpGet]
        public IEnumerable<AllConfigurationStringsModel> GetAllConfigurationStrings() => _configurationProvider.GetAllConfigurationStrings();
    }
}
