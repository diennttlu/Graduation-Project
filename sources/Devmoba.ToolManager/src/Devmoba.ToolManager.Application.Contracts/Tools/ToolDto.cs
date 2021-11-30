using Devmoba.ToolManager.ClientMachines;
using System;
using Volo.Abp.Application.Dtos;

namespace Devmoba.ToolManager.Tools
{
    public class ToolDto : EntityDto<long>
    {
        public string Name { get; set; }
        
        public Guid AppId { get; set; }

        public string Version { get; set; }

        public long ClientId { get; set; }

        public string ExeFilePath { get; set; }

        public int ProcessId { get; set; }

        public ProcessState ProcessState { get; set; }

        public DateTime LastUpdate { get; set; }

        public bool SentMail { get; set; }

        public ToolStatus ToolStatus { get; set; }

        public string Username { get; set; }

        public ClientMachineDto ClientMachine { get; set; }
    }

}
