using System;
using System.Collections.Generic;
using System.Text;

namespace Devmoba.ToolManager.HubConnections
{
    internal class Message
    {
        public long? ScriptId { get; set; }

        public List<string> Clients { get; set; }

        public List<long> Dependencies { get; set; }

        public string Content { get; set; }

        public bool Available { get; set; } = false;
    }
}
