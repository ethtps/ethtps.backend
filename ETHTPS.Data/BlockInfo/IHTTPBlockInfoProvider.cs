
namespace ETHTPS.Data.Core.BlockInfo
{
    /// <summary>
    /// Provides information about blocks.
    /// </summary>
    public interface IHTTPBlockInfoProvider : IInstantBlockInfoProvider, IHistoricalBlockInfoProvider
    {
        public double? BlockTimeSeconds { get; set; }
    }
}
