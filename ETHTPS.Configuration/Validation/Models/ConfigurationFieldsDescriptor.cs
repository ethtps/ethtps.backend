using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHTPS.Configuration.Validation.Models
{
    public class ConfigurationFieldsDescriptor
    {
        public string? Name { get; set; }
        public string[]? RequiredConfigurationStrings { get; set; }
    }
}
