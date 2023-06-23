using ETHTPS.Data.Core.Models.Pages.Chart;

namespace ETHTPS.Data.Core.Models.Pages.HomePage
{
    public sealed class HomePageRequestModel : RequestModelWithChartBase
    {
        public string SubchainsOf { get; set; } = Constants.All;
    }
}
