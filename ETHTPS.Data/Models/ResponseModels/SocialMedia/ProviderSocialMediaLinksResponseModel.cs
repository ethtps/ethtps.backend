using System.Collections.Generic;

namespace ETHTPS.Data.Core.Models.ResponseModels.SocialMedia
{
    public sealed class ProviderSocialMediaLinksResponseModel
    {
        /// <summary>
        /// Gets or sets the links. Keys represent the categories of the websites.
        /// </summary>
        public required Dictionary<string, IEnumerable<ProviderSocialMediaLink>> CategorizedLinks { get; set; }
    }
}
