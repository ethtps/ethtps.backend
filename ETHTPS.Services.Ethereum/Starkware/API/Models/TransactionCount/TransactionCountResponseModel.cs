using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum.Starkware.API.Models.TransactionCount
{
    public sealed class TransactionCountResponseModel
    {
        [JsonProperty("count")]
        public int Count { get; set; }
    }

}
