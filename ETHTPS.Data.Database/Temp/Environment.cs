using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL.Temp
{
    public partial class Environment
    {
        public Environment()
        {
            MicroserviceConfigurationStrings = new HashSet<MicroserviceConfigurationString>();
            ProviderConfigurationStrings = new HashSet<ProviderConfigurationString>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<MicroserviceConfigurationString> MicroserviceConfigurationStrings { get; set; }
        public virtual ICollection<ProviderConfigurationString> ProviderConfigurationStrings { get; set; }
    }
}
