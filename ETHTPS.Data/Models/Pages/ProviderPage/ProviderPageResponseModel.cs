using System.Collections.Generic;

using ETHTPS.Data.Core.Models.Pages.Chart;

namespace ETHTPS.Data.Core.Models.Pages.ProviderPage
{
    public class ProviderPageResponseModel : ResponseModelWithChartBase
    {
        public IEnumerable<TimeInterval> IntervalsWithData { get; set; }
        public IEnumerable<string> UniqueDataYears { get; set; }
    }
}
