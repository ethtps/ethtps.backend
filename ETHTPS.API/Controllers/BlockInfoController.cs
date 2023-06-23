using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ETHTPS.API.Core.Attributes;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers
{
    [Route("/api/v3/BlockInfo/[action]")]
    [ApiController]
    [Authorize]
    public sealed class BlockInfoController : ControllerBase
    {
        private readonly IAsyncHistoricalBlockInfoProvider _asyncHistoricalBlockInfoProvider;

        public BlockInfoController(IAsyncHistoricalBlockInfoProvider asyncHistoricalBlockInfoProvider)
        {
            _asyncHistoricalBlockInfoProvider = asyncHistoricalBlockInfoProvider;
        }

        [HttpGet]
        [TTL(10)]
        public async Task<IEnumerable<IBlock>> GetBlocksBetweenAsync(ProviderQueryModel model, DateTime start, DateTime end)
        {
            return await _asyncHistoricalBlockInfoProvider.GetBlocksBetweenAsync(model, start, end);
        }

        [HttpGet]
        [TTL(10)]
        public async Task<IEnumerable<IBlock>> GetLatestBlocksAsync(ProviderQueryModel model, string period)
        {
            var result = TryParse(period);
            if (result == null)
            {
                return (IEnumerable<IBlock>)Task.FromResult((IAsyncEnumerable<IBlock>)BadRequest());
            }
            return await _asyncHistoricalBlockInfoProvider.GetLatestBlocksAsync(model, result.Value);
        }

        private static TimeInterval? TryParse(string value)
        {
            TimeInterval result;
            bool ok = Enum.TryParse<TimeInterval>(value, out result);
            return ok ? result : null;
        }
    }
}
