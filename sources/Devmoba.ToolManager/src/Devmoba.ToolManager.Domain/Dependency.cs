using Volo.Abp.Domain.Entities;

namespace Devmoba.ToolManager
{
    public class Dependency : Entity<long>
    {
        public long ScriptId { get; set; }

        public long ScriptDependencyId { get; set; }

        public virtual Script Script { get; set; }

        public virtual Script ScriptDependency { get; set; }
    }
}
