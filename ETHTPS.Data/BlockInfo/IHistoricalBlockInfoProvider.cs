﻿using System;
using System.Threading.Tasks;

using ETHTPS.Data.Core.Models.DataEntries;

namespace ETHTPS.Data.Core.BlockInfo
{
    /// <summary>
    /// Provides information about blocks.
    /// </summary>
    public interface IHistoricalBlockInfoProvider
    {
        /// <summary>
        ///  Gets information about a block asynchronously.
        /// </summary>
        /// <param name="blockNumber">The number of the block.</param>
        Task<Block> GetBlockInfoAsync(int blockNumber);

        /// <summary>
        /// Gets information about a block asynchronously.
        /// </summary>
        /// <param name="time">The time of the block.</param>
        Task<Block> GetBlockInfoAsync(DateTime time);
    }
}
