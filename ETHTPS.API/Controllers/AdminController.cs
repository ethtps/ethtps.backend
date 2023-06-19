
using System.Collections.Generic;

using ETHTPS.Configuration;
using ETHTPS.Configuration.Database;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers
{
    [Route("/api/v3/admin/[action]")]
    [ApiController]
    [Authorize/*(Roles = "Admin")*/]
    public sealed class AdminController : ControllerBase
    {
        private readonly IDBConfigurationProvider _configurationProvider;

        public AdminController(IDBConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        [HttpGet]
        public IEnumerable<AllConfigurationStringsModel> GetAllConfigurationStrings() => _configurationProvider.GetAllConfigurationStrings();

        [HttpGet]
        public IEnumerable<string>? GetAllEnvironments() => _configurationProvider.GetEnvironments();

        [HttpGet]
        public IEnumerable<IMicroservice>? GetAllMicroservices() => _configurationProvider.GetMicroservices();
    }
}
