using Devmoba.ReportToToolManager.Exceptions;
using Devmoba.ReportToToolManager.Models;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Devmoba.ReportToToolManager
{
    public class ReportTool : IReportTool
    {
        private const string ReportApiEndpoint = "/api/tools/report";
        private readonly string _serverAddress;

        public ReportTool(string serverAddress)
        {
            _serverAddress = serverAddress;
        }

        public void Report(string name, Guid appId, string version, string exeFilePath, int processId)
        {
            var tool = new Tool()
            {
                Name = name,
                AppId = appId,
                Version = version,
                ExeFilePath = exeFilePath,
                ProcessId = processId,
                IPLan = GetIPLan()
            };
            ExecuteRequest(ReportApiEndpoint, tool);
        }

        #region private method
        private RestRequest CreatePostRequest(string resource)
        {
            var request = new RestRequest(resource, Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("accept", "text/plain");
            request.Timeout = 10000;
            return request;
        }

        private void ExecuteRequest(string apiEndPoint, object parameter)
        {
            var request = CreatePostRequest(apiEndPoint);
            request.AddJsonBody(parameter);
            var server = new RestClient(_serverAddress);
            var rs = server.Execute(request);
            if (!rs.IsSuccessful)
            {
                var errorDetails = GetErrorDetails(rs);
                throw new ReportFailedException(ReportApiEndpoint, rs.StatusCode, errorDetails);
            }
        }

        private string GetErrorDetails(IRestResponse response)
        {
            return $"* Transport or other non-HTTP error generated while attempting request: {response.ErrorMessage}\r\n* Remote server response: {response.Content}";
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
        #endregion
    }
}
