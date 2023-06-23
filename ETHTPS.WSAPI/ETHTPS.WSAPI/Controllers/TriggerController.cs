using ETHTPS.Data.Core.Models.LiveData.Triggers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace ETHTPS.WSAPI.Controllers
{
    [Route("/api/v3/wsapi/trigger/[action]")]
    //[Authorize(Roles = "Microservice")]
    [AllowAnonymous]
    public sealed class TriggerController : Controller
    {
        private readonly ILogger<TriggerController> _logger;

        public TriggerController(ILogger<TriggerController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult RegisterUpdate([FromBody] L2DataUpdateModel data)
        {
            _logger.LogInformation($"Received new data entry: " + JsonConvert.SerializeObject(data));
            return StatusCode(201);
        }
    }
}
