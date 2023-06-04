
using ETHTPS.Data.Core.Models.ResponseModels.SocialMedia;

using Newtonsoft.Json;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class ExternalWebsite : ExternalWebsiteBase
{
    public virtual ExternalWebsiteCategory? CategoryNavigation { get; set; } = new();

    [JsonIgnore]
    public virtual ICollection<ProviderLink> ProviderLinks { get; } = new List<ProviderLink>();
}
