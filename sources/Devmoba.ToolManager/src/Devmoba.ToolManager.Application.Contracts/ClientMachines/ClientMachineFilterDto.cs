using Volo.Abp.Application.Dtos;

namespace Devmoba.ToolManager.ClientMachines
{
    public class ClientMachineFilterDto : PagedAndSortedResultRequestDto
    {
        public long? Id { get; set; }

        public string Username { get; set; }

        public string IpLan { get; set; }

        public string IpPublic { get; set; }

        public ClientStatus? ClientStatus { get; set; }
    }
}
