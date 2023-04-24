using ETHTPS.Data.Core.Models.LiveData.Triggers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.WSAPI.Controllers
{
    [Route("/api/v3/wsapi/trigger/[action]")]
    [Authorize(Roles = "Microservice")]
    public sealed class TriggerController : Controller
    {
        [HttpPost]
        public IActionResult RegisterUpdate([FromBody] L2DataUpdateModel data)
        {
            return Content("test");
        }
    }
}
