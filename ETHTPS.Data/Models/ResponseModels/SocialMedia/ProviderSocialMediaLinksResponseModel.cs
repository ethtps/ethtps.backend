using System.Collections.Generic;

namespace ETHTPS.Data.ResponseModels.SocialMedia
{
    public sealed class ProviderSocialMediaLinksResponseModel
    {
        public IEnumerable<ProviderSocialMediaLink> Links { get; set; }
        public IEnumerable<ExternalWebsiteBase> ExternalWebsites { get; set; }
    }
}
