using Devmoba.ToolManager.Tools;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Devmoba.ToolManager.ClientMachines
{
    public class ClientMachineReportEmailDto : EntityDto<long>
    {
        public string IPLan { get; set; }

        public string IPPublic { get; set; }

        public List<ToolDto> ToolInactives { get; set; }

        public int TotalTool { get; set; }
    }
}
