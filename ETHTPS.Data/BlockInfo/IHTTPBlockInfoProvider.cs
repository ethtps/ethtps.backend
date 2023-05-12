
namespace ETHTPS.Data.Core.BlockInfo
{
    /// <summary>
    /// Provides information about blocks.
    /// </summary>
    public interface IHTTPBlockInfoProvider : IInstantBlockInfoProvider, IHistoricalBlockInfoProvider
    {
        /// <summary>
        /// An optional value that can be used to specify the time it takes to mine a block.
        /// </summary>
        public double? BlockTimeSeconds { get; set; }
    }
}
