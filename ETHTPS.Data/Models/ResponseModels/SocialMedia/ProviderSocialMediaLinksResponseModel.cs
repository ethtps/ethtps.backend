using System.Collections.Generic;

namespace ETHTPS.Data.Core.Models.ResponseModels.SocialMedia
{
    public sealed class ProviderSocialMediaLinksResponseModel
    {
        public IEnumerable<ProviderSocialMediaLink> Links { get; set; }
        public IEnumerable<ExternalWebsiteBase> ExternalWebsites { get; set; }
    }
}
