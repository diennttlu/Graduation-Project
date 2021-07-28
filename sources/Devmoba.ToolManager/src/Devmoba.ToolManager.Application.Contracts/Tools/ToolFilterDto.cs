using Volo.Abp.Application.Dtos;

namespace Devmoba.ToolManager.Tools
{
    public class ToolFilterDto : PagedAndSortedResultRequestDto
    {
        public long? Id { get; set; }

        public string Name { get; set; }

        public string AppId { get; set; }

        public string Version { get; set; }

        public ToolStatus? ToolStatus { get; set; }

        public long? ClientId { get; set; }
    }
}
