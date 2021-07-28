using Devmoba.ToolManager.Scripts;
using Volo.Abp.Application.Dtos;

namespace Devmoba.ToolManager.Dependencies
{
    public class DependencyDto : EntityDto<long>
    {
        public long ScriptId { get; set; }

        public long ScriptDependencyId { get; set; }

        public ScriptDto Script { get; set; }

        public ScriptDto ScriptDependency { get; set; }
    }
}
