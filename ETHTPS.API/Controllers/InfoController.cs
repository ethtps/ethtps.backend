using ETHTPS.API.BIL.Infrastructure.Services;
using ETHTPS.API.Core.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ETHTPS.API.Controllers
{
    [Route("/api/v3/info/[action]")]
    [Authorize]
    [ApiController]
    public sealed class InfoController : APIControllerBase
    {
        private readonly IInfoService _infoService;

        public InfoController(IInfoService infoService)
        {
            _infoService = infoService;
        }


    }
}
