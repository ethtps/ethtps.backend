using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ETHTPS.Data.Core.Models.Queries.Data.Requests
{
    public enum L2DataRequestState { Pending, Processing, Failed, Completed }
    public sealed class L2DataRequestStatus
    {
        public double ProgressPercent { get; set; } = 0;
        [JsonConverter(typeof(StringEnumConverter))]
        public L2DataRequestState State { get; set; } = L2DataRequestState.Pending;
        public const string PREFIX = "L2DataRequestStatus:";
    }
}
