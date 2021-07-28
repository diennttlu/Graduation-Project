using Devmoba.ToolManager.Scripts;
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

        Task TurnOnTool(string connectionId, string exeFilePath);

        Task TurnOffTool(string connectionId, long toolId, int processId);
    }
}
