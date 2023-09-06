using ETHTPS.Core;
using ETHTPS.Data.Core.Extensions.StringExtensions;

namespace ETHTPS.Data.Core.Models.Queries.Data.Requests
{
    /// <summary>
    /// Represents a filtering model based on a provider, network and whether to include sidechains. Also has an APIKey property.
    /// </summary>
    public class ProviderQueryModel : ICachedKey
    {
        /// <summary>
        /// All hell breaks loose if we don't initially set this to "All" :\
        /// </summary>
        public string Provider { get; set; } = Constants.All;
        public string Network { get; set; } = Constants.Mainnet;
        public bool IncludeSidechains { get; set; } = true;

        public static ProviderQueryModel FromProviderName(string provider) => new()
        {
            Provider = provider.RemoveAllNonAlphaNumericCharacters() // Avoid sql injection
        };

        public static ProviderQueryModel All => new()
        {
            Provider = Constants.All
        };

        public string ToCacheKey()
        {
            return $"{Provider}:{Network}:{IncludeSidechains}";
        }
    }
}
