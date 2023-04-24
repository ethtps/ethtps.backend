
using System.Collections.Generic;
using System.Threading.Tasks;

using ETHTPS.API.BIL.Infrastructure.Services;
using ETHTPS.API.Core.Attributes;
using ETHTPS.Data.Core.Models;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers
{
    [Route("/api/v3/Experiments/[action]")]
    [ApiController]
    public class ExperimentController : ControllerBase
    {
        private readonly IExperimentService _experimentService;

        public ExperimentController(IExperimentService experimentService)
        {
            _experimentService = experimentService;
        }

        [HttpGet]
        [TTL(60)]
        public async Task<IEnumerable<int>> GetAvailableExperiments([FromQuery] ExperimentRequesterParameters parameters)
        {
            return await _experimentService.GetAvailableExperimentsAsync(parameters, HttpContext);
        }
    }
}
