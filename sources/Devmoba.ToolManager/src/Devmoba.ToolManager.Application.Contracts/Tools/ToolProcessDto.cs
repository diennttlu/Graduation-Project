using System;
using Volo.Abp.Application.Dtos;

namespace Devmoba.ToolManager.Tools
{
    public class ToolProcessDto : EntityDto<long>
    {
        public string Name { get; set; }

        //public int ProcessId { get; set; }

        public ProcessState ProcessState { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
