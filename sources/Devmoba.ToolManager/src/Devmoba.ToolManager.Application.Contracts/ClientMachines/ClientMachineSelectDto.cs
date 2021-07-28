using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Devmoba.ToolManager.ClientMachines
{
    public class ClientMachineSelectDto : EntityDto<long>
    {
        public string Username { get; set; }
    }
}
