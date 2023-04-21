using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHTPS.Configuration.Validation.Models
{
    public abstract class ConfigurationModelBase
    {
        public ConfigurationFieldsDescriptor[]? MicroserviceConfiguration { get; set; }
        public ConfigurationFieldsDescriptor[]? ProviderConfiguration { get; set; }
    }
}
