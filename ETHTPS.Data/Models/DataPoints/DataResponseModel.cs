using System.Collections.Generic;

using Newtonsoft.Json;

namespace ETHTPS.Data.Core.Models.DataPoints
{
    public class DataResponseModel
    {
        public List<DataPoint> Data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Provider { get; set; }
    }
}
