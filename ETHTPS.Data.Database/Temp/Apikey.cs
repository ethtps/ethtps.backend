using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL.Temp
{
    public partial class Apikey
    {
        public Apikey()
        {
            ApikeyExperimentBindings = new HashSet<ApikeyExperimentBinding>();
            ApikeyGroups = new HashSet<ApikeyGroup>();
        }

        public int Id { get; set; }
        public string KeyHash { get; set; } = null!;
        public int TotalCalls { get; set; }
        public int CallsLast24h { get; set; }
        public int Limit24h { get; set; }
        public string RequesterIpaddress { get; set; } = null!;

        public virtual ICollection<ApikeyExperimentBinding> ApikeyExperimentBindings { get; set; }
        public virtual ICollection<ApikeyGroup> ApikeyGroups { get; set; }
    }
}
