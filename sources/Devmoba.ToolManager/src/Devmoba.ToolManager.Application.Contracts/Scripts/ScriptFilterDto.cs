using Volo.Abp.Application.Dtos;

namespace Devmoba.ToolManager.Scripts
{
    public class ScriptFilterDto : PagedAndSortedResultRequestDto
    {
        public long? Id { get; set; }

        public string Name { get; set; }
    }
}
