using Devmoba.ToolManager.Dependencies;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Devmoba.ToolManager.Scripts
{
    public class ScriptDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Content { get; set; }

        public string Comment { get; set; }

        public ICollection<DependencyDto> Dependencies { get; set; }

        public ICollection<DependencyDto> ScriptDependencies { get; set; }
    }
}
