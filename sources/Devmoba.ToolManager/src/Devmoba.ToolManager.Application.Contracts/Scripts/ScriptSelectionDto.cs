using Volo.Abp.Application.Dtos;

namespace Devmoba.ToolManager.Scripts
{
    public class ScriptSelectionDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Comment { get; set; }

        public bool Checked { get; set; } = false;
    }
}
