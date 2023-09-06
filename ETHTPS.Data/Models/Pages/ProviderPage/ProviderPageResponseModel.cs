using System.Collections.Generic;

using ETHTPS.Data.Core.Models.Pages.Chart;
using ETHTPS.Data.Core.Models.ResponseModels.SocialMedia;

namespace ETHTPS.Data.Core.Models.Pages.ProviderPage
{
    public sealed class ProviderPageResponseModel : ResponseModelWithChartBase
    {
        public IEnumerable<TimeInterval> IntervalsWithData { get; set; }
        public IEnumerable<string> UniqueDataYears { get; set; }
        public required ProviderSocialMediaLinksResponseModel Links { get; set; }
    }
}
