using Devmoba.ToolClient.Shareds;
using System;

namespace Devmoba.ToolClient.Models
{
    public class ToolProcess
    {
        public long Id { get; set; }

        public string Name { get; set; }

        //public int ProcessId { get; set; }

        public ProcessState ProcessState { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
