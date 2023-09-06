
using System.Collections.Generic;
using System.Threading.Tasks;

using ETHTPS.API.Core.Attributes;
using ETHTPS.API.Core.Integrations.MSSQL.Services;
using ETHTPS.Data.Core.Models;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers
{
    [Route("/api/v3/Experiments/[action]")]
    [ApiController]
    [Authorize]
    public sealed class ExperimentController : ControllerBase
    {
        private readonly ExperimentService _experimentService;

        public ExperimentController(ExperimentService experimentService)
        {
            _experimentService = experimentService;
        }

        [HttpGet]
        [TTL(2)]
        public async Task<IEnumerable<int>> GetAvailableExperiments([FromQuery] ExperimentRequesterParameters parameters)
        {
            return await _experimentService.GetAvailableExperimentsAsync(parameters, HttpContext);
        }

        [HttpGet]
        [TTL(2)]
        public async Task<Experiment?> GetExperiment(int id)
        {
            return await _experimentService.GetExperimentByIDAsync(id);
        }
    }
}
