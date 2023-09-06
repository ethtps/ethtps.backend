using ETHTPS.Data.Core.Models.ExternalWebsites;

namespace ETHTPS.Data.Core.Models.ResponseModels.SocialMedia
{
    public abstract class ExternalWebsiteBase : IExternalWebsite
    {
        public string Name { get; set; }

        public string IconBase64 { get; set; }

        public int Category { get; set; }
        public int Id { get; set; }
    }
}
