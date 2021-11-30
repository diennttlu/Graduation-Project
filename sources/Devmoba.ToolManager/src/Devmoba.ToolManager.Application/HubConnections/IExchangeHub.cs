using Devmoba.ToolManager.Scripts;
using Devmoba.ToolManager.Tools;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Devmoba.ToolManager.HubConnections
{
    public interface IExchangeHub
    {
        Task ReceiveFromUser(string connectionId, List<ScriptDto> dependencies, string content, bool available);

        Task ReceiveFromClient(object obj);

        Task ReceiveFromTool(object obj);

        Task ReceiveClientInfo(object obj);

        Task ReloadClientTable();

        Task ReloadToolTable();

        Task ReceiveError(object obj);

        Task ReceiveScript(long id, string content);

        Task ReceiveScriptDependencyIds(List<long> dependencies);

        Task ReceiveTimestamp(long timestamp);

        Task ReceiveToolProcesses(List<ToolProcessDto> tools);

        Task TurnOnTool(string connectionId, long toolId, string exeFilePath);

        Task TurnOffTool(string connectionId, long toolId, int processId);
    }
}
