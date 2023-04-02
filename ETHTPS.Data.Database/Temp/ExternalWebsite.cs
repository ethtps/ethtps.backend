using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL.Temp
{
    public partial class ExternalWebsite
    {
        public ExternalWebsite()
        {
            ProviderLinks = new HashSet<ProviderLink>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string IconBase64 { get; set; } = null!;
        public int Category { get; set; }

        public virtual ExternalWebsiteCateopry CategoryNavigation { get; set; } = null!;
        public virtual ICollection<ProviderLink> ProviderLinks { get; set; }
    }
}
