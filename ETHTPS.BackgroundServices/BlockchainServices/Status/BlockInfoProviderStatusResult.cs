using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ETHTPS.Services.BlockchainServices.Status
{
    public sealed class BlockInfoProviderStatusResult
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public BlockInfoProviderStatus Status { get; set; }

        public string Details { get; set; }
    }
}
