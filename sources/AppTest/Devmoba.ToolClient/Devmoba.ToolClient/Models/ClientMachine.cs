using Devmoba.ToolClient.Shareds;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Devmoba.ToolClient.Models
{
    public class ClientMachine
    {
        public string IPLan { get; set; }

        public double CPUPercentage { get; set; }

        public double MemoryPercentage { get; set; }
    }
}
