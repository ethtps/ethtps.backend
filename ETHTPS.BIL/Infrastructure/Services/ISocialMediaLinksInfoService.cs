using ETHTPS.Data.ResponseModels.SocialMedia;

namespace ETHTPS.API.BIL.Infrastructure.Services
{
    public interface ISocialMediaLinksInfoService
    {
        public ProviderSocialMediaLinksResponseModel GetProviderSocialMediaLinks(string providerName);
    }
}
