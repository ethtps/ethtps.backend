
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using ETHTPS.Configuration;
using ETHTPS.Configuration.Database;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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

        [HttpPut]
        public IActionResult AddOrUpdateConfigurationString(
            [FromBody] ConfigurationStringUpdateModel configurationStringModel, string? microservice = null, string? environment = null)
        {
            try
            {
                var id = _configurationProvider.AddOrUpdateConfigurationString(configurationStringModel, microservice,
                    environment);
                return Created($"/strings/{id}", id);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("not provided"))
                {
                    return BadRequest("Missing microservice or environment");
                }
            }

            throw new Exception("Unable to update configuration string");
        }

        [HttpGet]
        public ConfigurationStringLinksModel GetAllConfigurationStringLinks(int configurationStringID) => _configurationProvider.GetAllLinks(configurationStringID);

        [HttpDelete]
        public IActionResult ClearHangfireQueue()
        {
            var id = _configurationProvider.ClearHangfireQueue();
            return Content(id.ToString());
        }
    }
}
