﻿using System.Threading.Tasks;

using ETHTPS.Data.Core.Models.DataEntries;

namespace ETHTPS.Data.Core.BlockInfo
{
    /// <summary>
    /// Provides information about blocks.
    /// </summary>
    public interface IInstantBlockInfoProvider
    {
        /// <summary>
        /// Gets information about the latest block asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<Block> GetLatestBlockInfoAsync();
    }
}
