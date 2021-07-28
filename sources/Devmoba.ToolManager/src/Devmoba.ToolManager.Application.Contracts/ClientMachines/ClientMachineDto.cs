using Devmoba.ToolManager.Tools;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Devmoba.ToolManager.ClientMachines
{
    public class ClientMachineDto : EntityDto<long>
    {
        public string IPLan { get; set; }

        public string IPPublic { get; set; }

        public DateTime LastUpdate { get; set; }

        public ClientStatus ClientStatus { get; set; }

        public Guid UserId { get; set; }

        public string Username { get; set; }

        public ICollection<ToolDto> Tools { get; set; }
    }
}
