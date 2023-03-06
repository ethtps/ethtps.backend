using ETHTPS.Services.BlockchainServices;

namespace ETHTPS.API.BIL.Infrastructure.Services.BlockInfo
{
    /// <summary>
    /// Provides information about blocks.
    /// </summary>
    public interface IHTTPBlockInfoProvider : IInstantBlockInfoProvider, IHistoricalBlockInfoProvider
    {
        public double BlockTimeSeconds { get; set; }
    }
}
