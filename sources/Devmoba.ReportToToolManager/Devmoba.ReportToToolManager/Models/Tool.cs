using System;

namespace Devmoba.ReportToToolManager.Models
{
    internal class Tool
    {
        public string Name { get; set; }

        public Guid AppId { get; set; }
    
        public string Version { get; set; }

        public string IPLan { get; set; }

        public string ExeFilePath { get; set; }

        public int ProcessId { get; set; }
    }
}
