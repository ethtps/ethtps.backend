using ETHTPS.API.Core.Controllers;
using ETHTPS.Configuration.Database.Initialization;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers
{
#if DEBUG
    [Route("api/v3/[controller]")]
    public sealed class DataInitialization : APIControllerBase
    {
        private readonly PublicDataInitializer _publicDataInitializer;

        public DataInitialization(PublicDataInitializer publicDataInitializer)
        {
            _publicDataInitializer = publicDataInitializer;
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult Initialize()
        {
            _publicDataInitializer.Initialize();
            return Created(string.Empty, null);
        }
    }
#endif
}
