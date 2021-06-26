using Devmoba.ToolClient.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Devmoba.ToolClient.Shareds
{
    public static class CommonMethods
    {
        public static string GetIPLan()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ip = host.AddressList
                .Where(x => x.AddressFamily == AddressFamily.InterNetwork)
                .Select(x => x.ToString())
                .FirstOrDefault();
            return ip;
        }

        public static double GetCpuPercentage()
        {
            var cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuUsage.NextValue();
            var cpuPercent = cpuUsage.NextValue();
            return Math.Round(cpuPercent, 2);
        }

        public static double GetMemoryPercentage()
        {
            var memoryUsage = new PerformanceCounter("Memory", "% Committed Bytes In Use", string.Empty);
            return Math.Round(memoryUsage.NextValue(), 2);
        }
    }
}
