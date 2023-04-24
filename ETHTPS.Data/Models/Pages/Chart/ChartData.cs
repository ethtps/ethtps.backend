using System.Collections.Generic;

using ETHTPS.Data.Core.Models.DataPoints;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ETHTPS.Data.Core.Models.Pages.Chart
{
    public class ChartData : IChartDataType
    {
        public IDictionary<string, IEnumerable<DataResponseModel>> Data { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DataType DataType { get; set; }
    }
}
