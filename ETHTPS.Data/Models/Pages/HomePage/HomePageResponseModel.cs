using System.Collections.Generic;

using ETHTPS.Data.Core.Models.Pages.Chart;
using ETHTPS.Data.Core.Models.ResponseModels;

namespace ETHTPS.Data.Core.Models.Pages.HomePage
{
    public sealed class HomePageResponseModel : ResponseModelWithChartBase
    {
        public IDictionary<string, object> MaxData { get; set; }
        public IDictionary<string, object> InstantData { get; set; }
        public IDictionary<string, string> ColorDictionary { get; set; }
        public IDictionary<string, string> ProviderTypesColorDictionary { get; set; }
        public IEnumerable<ProviderResponseModel> Providers { get; set; }
    }
}
