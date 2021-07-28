using System.Collections.Generic;

namespace Devmoba.ToolClient.Models
{
    public class MainScript
    {
        //public long? ScriptId { get; set; }

        public string Content { get; set; }

        public List<Script> Dependencies { get; set; }

        public bool Available { get; set; }
    }
}
