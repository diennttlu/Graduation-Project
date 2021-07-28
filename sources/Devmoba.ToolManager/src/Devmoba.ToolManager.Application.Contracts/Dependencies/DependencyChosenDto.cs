using System;
using System.Collections.Generic;
using System.Text;

namespace Devmoba.ToolManager.Dependencies
{
    public class DependencyChosenDto
    {
        public long ScriptId { get; set; }

        public long ScriptDependencyId { get; set; }

        public bool Checked { get; set; }

        public bool InDb { get; set; }
    }
}
