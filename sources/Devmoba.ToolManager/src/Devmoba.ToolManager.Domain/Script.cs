using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Devmoba.ToolManager
{
    public class Script : Entity<long>
    {
        public string Name { get; set; }

        public string Content { get; set; }

        public string Comment { get; set; }

        public virtual ICollection<Dependency> Dependencies { get; set; }

        public virtual ICollection<Dependency> ScriptDependencies { get;set; }

    }
}
