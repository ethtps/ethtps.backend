using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ETHTPS.Data.Core.Models.Queries.Data.Requests
{
    public enum L2DataRequestState { Pending, Processing, Failed, Completed }

    public sealed class L2DataRequestStatus : ICachedKey, IGuidEntity
    {
        public L2DataRequestStatus(string guid)
        {
            Guid = guid;
        }

        public L2DataRequestStatus(double progressPercent, L2DataRequestState state, string guid)
        {
            ProgressPercent = progressPercent;
            State = state;
            Guid = guid;
        }

        public double ProgressPercent { get; set; } = 0;

        [JsonConverter(typeof(StringEnumConverter))]
        public L2DataRequestState State { get; set; } = L2DataRequestState.Pending;
        public string Guid { get; set; }

        public static string GenerateCacheKeyFromGuid(string guid) => $"L2DataRequestStatus:{guid}";

        public string ToCacheKey() => GenerateCacheKeyFromGuid(Guid);
    }
}
