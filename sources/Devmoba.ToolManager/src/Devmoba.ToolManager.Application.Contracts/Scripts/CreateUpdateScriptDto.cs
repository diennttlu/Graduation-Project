using Devmoba.ToolManager.Dependencies;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Devmoba.ToolManager.Scripts
{
    public class CreateUpdateScriptDto
    {
        [Required]
        [StringLength(512)]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }

        public string Comment { get; set; }

        public List<long> DependencyIds { get; set; }

        public List<DependencyChosenDto> DependencyChosens { get; set; }
    }
}
