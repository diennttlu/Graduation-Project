using System;

namespace Devmoba.ReportToToolManager
{
    public interface IReportTool
    {
        /// <summary>
        /// Report đến Tool Manager để bảo tool còn hoạt động
        /// </summary>
        /// <param name="name"></param>
        /// <param name="appId"></param>
        /// <param name="version"></param>
        /// <param name="exeFilePath"></param>
        /// <param name="processId"></param>
        /// <exception cref="Devmoba.ReportToToolManager.Exceptions.ReportFailedException"></exception>
        void Report(string name, Guid appId, string version, string exeFilePath, int processId);
    }
}
