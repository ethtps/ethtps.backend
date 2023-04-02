using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL.Temp
{
    public partial class Project
    {
        public Project()
        {
            Features = new HashSet<Feature>();
        }

        public int Id { get; set; }
        public bool Enabled { get; set; }
        public int? Provider { get; set; }
        public string Name { get; set; } = null!;
        public string Website { get; set; } = null!;
        public string? Details { get; set; }

        public virtual Provider? ProviderNavigation { get; set; }
        public virtual ICollection<Feature> Features { get; set; }
    }
}
