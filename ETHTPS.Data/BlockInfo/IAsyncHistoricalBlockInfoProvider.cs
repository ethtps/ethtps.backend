using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ETHTPS.Data.Core.BlockInfo
{
    public interface IAsyncHistoricalBlockInfoProvider
    {
        Task<IEnumerable<IBlock>> GetLatestBlocksAsync(ProviderQueryModel model, TimeInterval period);
        Task<IEnumerable<IBlock>> GetBlocksBetweenAsync(ProviderQueryModel model, DateTime start, DateTime end);
    }
}
