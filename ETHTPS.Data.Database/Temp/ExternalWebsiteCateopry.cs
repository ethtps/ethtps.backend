using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL.Temp
{
    public partial class ExternalWebsiteCateopry
    {
        public ExternalWebsiteCateopry()
        {
            ExternalWebsites = new HashSet<ExternalWebsite>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<ExternalWebsite> ExternalWebsites { get; set; }
    }
}
