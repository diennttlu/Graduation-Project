using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Devmoba.ToolManager.Dependencies
{
    public class CreateUpdateDependencyDto
    {
        [Required]
        public long ScriptId { get; set; }

        [Required]
        public long ScriptDependencyId { get; set; }
    }
}
