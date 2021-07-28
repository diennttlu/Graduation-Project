using Devmoba.ToolManager.Tools;

namespace Devmoba.ToolManager.ClientMachines
{
    public class ClientMachineOverviewDto
    {
        public string ClientName { get; set; }

        public string IPLan { get; set; }

        public string IPPublic { get; set; }

        public ClientStatus ClientStatus { get; set; }

        public ToolReportDto ToolReport { get; set; }

    }
}
