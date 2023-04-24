﻿using System.Collections.Generic;

namespace ETHTPS.Data.ResponseModels.SocialMedia
{
    public class ProviderSocialMediaLinksResponseModel
    {
        public IEnumerable<ProviderSocialMediaLink> Links { get; set; }
        public IEnumerable<ExternalWebsiteBase> ExternalWebsites { get; set; }
    }
}
