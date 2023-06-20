using ETHTPS.Data.Core.Models.DataUpdater;

using Newtonsoft.Json;

namespace ETHTPS.Data.Core.Models.ResponseModels
{
    public sealed class ProviderResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int TheoreticalMaxTPS { get; set; }
        public string Type { get; set; }
        public bool IsGeneralPurpose { get; set; }
        public string IsSubchainOf { get; set; }
        public IBasicLiveUpdaterStatus Status { get; set; }
        [JsonIgnore]
        public bool Enabled { get; set; }
    }
}
