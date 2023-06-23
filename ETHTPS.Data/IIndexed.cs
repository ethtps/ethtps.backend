using Newtonsoft.Json;

namespace ETHTPS.Data.Core
{
    public interface IIndexed
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
