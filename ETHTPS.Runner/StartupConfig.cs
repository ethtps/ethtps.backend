using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHTPS.Runner
{

    public class StartupConfig
    {
        public ExecutableDescriptor[]? Executables { get; set; }
        public string? BaseDirectory { get; set; }
    }

    public class ExecutableDescriptor
    {
        public string? Name { get; set; }
        public string? Directory { get; set; }
        public string? Executable { get; set; }
        public string[]? Arguments { get; set; }
    }

}
