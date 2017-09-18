using System.Collections.Generic;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Models.Commands
{
    public class Parameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class Command
    {
        public string Name { get; set; }
        public List<Parameter> Parameters { get; set; }
    }
}
