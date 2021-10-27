using System;
using System.Threading.Tasks;

namespace ReportToolManager.NetCore
{
    public interface IReportTool
    {
        /// <summary>
        /// Report đến Tool Manager để bảo tool còn hoạt động
        /// </summary>
        /// <param name="name"></param>
        /// <param name="appId"></param>
        /// <param name="version"></param>
        /// <exception cref="ReportToolManager.NetCore.Exceptions.ReportFailedException"></exception>
        Task ReportAsync(string name, Guid appId, string version);
    }
}
