using ETHTPS.Data.Core.Models.ResponseModels.SocialMedia;

namespace ETHTPS.API.BIL.Infrastructure.Services
{
    public interface ISocialMediaLinksInfoService
    {
        public ProviderSocialMediaLinksResponseModel GetProviderSocialMediaLinks(string providerName);
    }
}
