
using DalSoft.RestClient;
using ReportToolManager.NetFramework.Exceptions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ReportToolManager.NetFramework
{
    public class ReportTool : IReportTool
    {
        private readonly string _serverAddress;
        private readonly dynamic _restClient;

        public ReportTool(string serverAddress)
        {
            _serverAddress = serverAddress;
            _restClient = new RestClient(serverAddress);
        }

        public async Task ReportAsync(string name, Guid appId, string version)
        {
            HttpResponseMessage rs = await _restClient.api.tools.report.Post(new
            {
                name = name,
                appId = appId,
                version = version,
                exeFilePath = GetExeFilePath(),
                processId = GetProcessId(),
                ipLan = GetIPLan()
            });

            if (rs.StatusCode != HttpStatusCode.OK)
            {
                var errorDetails = GetErrorDetails(rs);
                throw new ReportFailedException("api/tools/report", rs.StatusCode, errorDetails);
            }
        }

        #region private method

        private string GetErrorDetails(HttpResponseMessage response)
        {
            return $"* Transport or other non-HTTP error generated while attempting request: {response.ReasonPhrase}\r\n* Remote server response: {response.Content}";
        }

        private string GetIPLan()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ip = host
                .AddressList
                .Where(x => x.AddressFamily == AddressFamily.InterNetwork)
                .Select(x => x.ToString())
                .FirstOrDefault();
            return ip;
        }

        private static string GetExeFilePath()
        {
            return Process.GetCurrentProcess().MainModule.FileName;
        }

        private static int GetProcessId()
        {
            return Process.GetCurrentProcess().Id;
        }
        #endregion
    }
}
