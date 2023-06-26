namespace ETHTPS.Data.Core.Models.ResponseModels.SocialMedia
{
    public sealed class ProviderSocialMediaLink
    {
        public required string Link { get; set; }
        public required string WebsiteName { get; set; }
        public string? WebsiteCategory { get; set; }
    }
}
