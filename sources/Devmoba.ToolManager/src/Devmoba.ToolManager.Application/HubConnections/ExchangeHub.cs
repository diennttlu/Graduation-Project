using Devmoba.ToolManager.ClientMachines;
using Devmoba.ToolManager.Dependencies;
using Devmoba.ToolManager.Localization;
using Devmoba.ToolManager.Scripts;
using Devmoba.ToolManager.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.SignalR;

namespace Devmoba.ToolManager.HubConnections
{
    [Authorize]
    [HubRoute("exchange-hub")]
    public class ExchangeHub : AbpHub<IExchangeHub>
    {
        private readonly static ConnectionMapping<string> _connections =
          new ConnectionMapping<string>();
        private readonly IConfiguration _cfg;
        private readonly IClientMachineAppService _clientMachineAppService;
        private readonly IScriptAppService _scriptAppService;
        private readonly IDependencyAppService _dependencyAppService;
        private readonly IToolAppService _toolAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<ToolManagerResource> _stringLocalizer;
        private readonly int _intervalTool;

        public ExchangeHub(
            IClientMachineAppService clientMachineAppService,
            IScriptAppService scriptAppService,
            IDependencyAppService dependencyAppService,
            IToolAppService toolAppService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration cfg,
            IStringLocalizer<ToolManagerResource> stringLocalizer)
        {
            _clientMachineAppService = clientMachineAppService;
            _scriptAppService = scriptAppService;
            _dependencyAppService = dependencyAppService;
            _toolAppService = toolAppService;
            _httpContextAccessor = httpContextAccessor;
            _stringLocalizer = stringLocalizer;
            _cfg = cfg;
            _intervalTool = _cfg.GetValue<int>(CommonContants.ToolInterval);
        }

        public override Task OnConnectedAsync()
        {
            _connections.Add(CurrentUser.UserName, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (CurrentUser.IsInRole(CommonContants.RoleNameClient))
            {
                var client = _clientMachineAppService.UpdateStatusAsync(CurrentUser.Id.Value, ClientStatus.Offline);

                Thread.Sleep(1000);
                if (client.IsCompleted)
                {
                    Clients.All.ReceiveClientInfo(new
                    {
                        connectionStatus = false,
                        clientId = client.Result.Id,
                    });
                }
            }
            _connections.Remove(CurrentUser.UserName, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        #region Run script
        public async Task SendClient(string message)
        {
            var ms = JsonConvert.DeserializeObject<Message>(message);
            if (ms.ScriptId.HasValue)
            {
                var script = await _scriptAppService.GetAsync(ms.ScriptId.Value);
                ms.Content = script.Content;
                ms.Dependencies = script.Dependencies.Select(x => x.ScriptDependencyId).ToList();
                ms.Available = true;
            }
            var dependencyScripts = new List<ScriptDto>();
            await GetDependencyScript(ms.Dependencies, dependencyScripts);
            foreach (var client in ms.Clients)
            {
                var connections = _connections.GetConnections(client);
                var connectionIdContext = Context.ConnectionId;
                if (connections != Enumerable.Empty<string>())
                {
                    await Clients.Clients(connections.ToList())
                        .ReceiveFromUser(
                            connectionIdContext,
                            dependencyScripts,
                            ms.Content,
                            ms.Available);
                }
                else
                {
                    await ReplyToUser(connectionIdContext, client, new List<string>(new string[] { $"{client} chưa được kết nối đến server!" }));
                }
            }
        }

        private async Task GetDependencyScript(List<long> dependencyIds, List<ScriptDto> scripts)
        {
            foreach (var id in dependencyIds)
            {
                var script = await _scriptAppService.GetAsync(id);
                if (script != null && !scripts.Where(x => x.Id == script.Id).Any())
                {
                    var dependencies = _dependencyAppService.GetScriptDependencyIdByScriptId(script.Id);
                    if (dependencies.Count > 0)
                    {
                        await GetDependencyScript(dependencies, scripts);
                    }
                    scripts.Add(new ScriptDto()
                    {
                        Id = script.Id,
                        Content = script.Content
                    });
                }
            }
        }

        public async Task ReplyToUser(string connectionId, string client, List<string> result)
        {
            await Clients.Client(connectionId).ReceiveFromClient(new { client = client, message = result });
        }
        #endregion

        #region Update client info
        public async Task UpdateClient(string ipLan, double cpuPercentage, double memoryPercentage)
        {
            var ipPublic = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var clientMachine = await _clientMachineAppService.GetByUserIdAsync(CurrentUser.Id.Value);
            try
            {
                var createUpdateClientMachie = new CreateUpdateClientMachineDto()
                {
                    IPLan = ipLan,
                    IPPublic = ipPublic,
                    LastUpdate = DateTime.UtcNow,
                    UserId = CurrentUser.Id.Value
                };

                if (clientMachine != null)
                {
                    createUpdateClientMachie.Timestamp = clientMachine.Timestamp;
                    await _clientMachineAppService.UpdateAsync(clientMachine.Id, createUpdateClientMachie);

                    await Clients.All.ReceiveClientInfo(new
                    {
                        connectionStatus = true,
                        clientId = clientMachine.Id,
                        ipLan = ipLan,
                        ipPublic = ipPublic,
                        clientStatus = ClientStatus.Online,
                        cpuPercentage = cpuPercentage,
                        memoryPercentage = memoryPercentage,
                        lastUpdate = DateTime.UtcNow
                    });
                }
                else
                {
                    //createUpdateClientMachie.Timestamp = CommonMethod.GetTimestamp();
                    clientMachine = await _clientMachineAppService.CreateAsync(createUpdateClientMachie);
                    await Clients.All.ReloadClientTable();
                }

                var connections = _connections.GetConnections(CurrentUser.UserName).ToList();
                await Clients.Clients(connections).ReceiveTimestamp(clientMachine.Timestamp);
            }
            catch (Exception ex)
            {
                var message = $"{CurrentUser.UserName} {ex.Message}";
                await Clients.All.ReceiveError(new { message = message });
            }
        }
        #endregion

        #region Process Handle

        public async Task GetToolProcess()
        {
            var clientMachine = await _clientMachineAppService.GetByUserIdAsync(CurrentUser.Id.Value);
            var tools = await _toolAppService.GetToolProcessesAsync(clientMachine.Id);
            var connections = _connections.GetConnections(CurrentUser.UserName).ToList();
            await Clients.Clients(connections).ReceiveToolProcesses(tools);
        }

        public async Task UpdateProcessState(List<ToolProcessDto> tools)
        {
            await _toolAppService.UpdateProcessesAsync(tools);
        }
        #endregion

        #region On/Off tool
        public async Task TurnOnTool(long id, string username)
        {
            var tool = await _toolAppService.GetAsync(id);
            if (tool.LastUpdate < DateTime.UtcNow.AddMinutes(-_intervalTool))
            {
                var connections = _connections.GetConnections(username).ToList();
                if (connections.Count > 0)
                    await Clients.Clients(connections).TurnOnTool(Context.ConnectionId, tool.Id, tool.ExeFilePath);
                else
                    throw new UserFriendlyException(_stringLocalizer["ClientOffline"]);
            }
        }

        public async Task TurnOffTool(long id, string username)
        {
            var tool = await _toolAppService.GetAsync(id);
            if (tool.LastUpdate >= DateTime.UtcNow.AddMinutes(-_intervalTool))
            {
                var connections = _connections.GetConnections(username).ToList();
                if (connections.Count > 0)
                    await Clients.Clients(connections).TurnOffTool(Context.ConnectionId, tool.Id, tool.ProcessId);
                else
                    throw new UserFriendlyException(_stringLocalizer["ClientOffline"]);
            }
        }

        public async Task ReportTurnOn(ReportMessage rm)
        {
            if (rm.IsSuccess)
            {
                await _toolAppService.UpdateStateAsync(
                    rm.ToolId,
                    DateTime.UtcNow,
                    ProcessState.Started,
                    true
                );
            }
            await Clients.Client(rm.ConnectionId).ReceiveFromClient(new
            {
                sw = SwitchTool.TurnOn,
                isSuccess = rm.IsSuccess,
                errorMessage = rm.ErrorMessage,
                toolId = rm.ToolId
            });
        }

        public async Task ReportTurnOff(ReportMessage rm)
        {
            if (rm.IsSuccess)
            {
                await _toolAppService.UpdateStateAsync(
                    rm.ToolId,
                    DateTime.UtcNow.AddMinutes(-_intervalTool),
                    ProcessState.Killed,
                    true
                );
            }
            await Clients.Client(rm.ConnectionId).ReceiveFromClient(new
            {
                sw = SwitchTool.TurnOff,
                isSuccess = rm.IsSuccess,
                errorMessage = rm.ErrorMessage,
                toolId = rm.ToolId
            });
        }
        #endregion

        public class ReportMessage
        {
            public string ConnectionId { get; set; }

            public long ToolId { get; set; }

            public bool IsSuccess { get; set; }

            public string ErrorMessage { get; set; }
        }
    }
}
