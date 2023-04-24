using System.Threading.Tasks;

using ETHTPS.API.BIL.Infrastructure.Services.APIKeys;
using ETHTPS.API.Security.Core.APIKeyProvider;
using ETHTPS.Data.ResponseModels.APIKey;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using StackExchange.Redis;

namespace ETHTPS.API.Controllers
{
    [Route("/api/v3/APIKeys/[action]")]
    [ApiController]
    [AllowAnonymous]
    public sealed class APIKeyController : ControllerBase, IAPIKeyService
    {
        private readonly IExtendedAPIKeyService _apiKeyService;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public APIKeyController(IExtendedAPIKeyService apiKeyService, IConnectionMultiplexer connectionMultiplexer)
        {
            _apiKeyService = apiKeyService;
            _connectionMultiplexer = connectionMultiplexer;
        }

        [HttpGet("GetNewKey")]
        public async Task<APIKeyResponseModel> RegisterNewKeyForProofAsync(string humanityProof)
        {
            return await _apiKeyService.RegisterNewKeyAsync(humanityProof, HttpContext.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());
        }

        [HttpGet]
        public async Task<string> RegdisGetTest(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return await db.StringGetAsync(key);
        }

        [HttpGet]
        public async Task<IActionResult> RegisSetTest(string key, string value)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.SetAddAsync(key, value);
            return Created();
        }
    }
}
