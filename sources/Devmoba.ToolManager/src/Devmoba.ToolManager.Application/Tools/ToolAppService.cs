using Devmoba.ToolManager.ClientMachines;
using Devmoba.ToolManager.HubConnections;
using Devmoba.ToolManager.Permissions;
using Devmoba.ToolManager.Repositories;
using Devmoba.ToolManager.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Devmoba.ToolManager.Tools
{
    [RemoteService(IsEnabled = false)]
    public class ToolAppService : ReadOnlyAppService<
        Tool,
        ToolDto, 
        long,
        ToolFilterDto>, IToolAppService
    {
        private new IToolRepository Repository;
        private readonly IConfiguration _cfg;
        private readonly int _toolInterval;
        private readonly int _clientInterval;
        private readonly IRepository<AppUser, Guid> _userRepository;
        private readonly IRepository<ClientMachine, long> _clientMachineRepository;
        private readonly IHubContext<ExchangeHub, IExchangeHub> _exchangeHub;

        public ToolAppService(
            IToolRepository repository,
            IConfiguration cfg,
            IRepository<AppUser, Guid> userRepository,
            IRepository<ClientMachine, long> clientMachineRepository,
            IHubContext<ExchangeHub, IExchangeHub> exchangeHub) : base(repository)
        {
            Repository = repository;
            _cfg = cfg;
            _toolInterval = _cfg.GetValue<int>(CommonContants.ToolInterval); // minutes
            _clientInterval = _cfg.GetValue<int>(CommonContants.ClientMachineInterval); 
            _userRepository = userRepository;
            _clientMachineRepository = clientMachineRepository;
            _exchangeHub = exchangeHub;
        }

        [Authorize(ToolManagerPermissions.Tools.Default)]
        public override async Task<PagedResultDto<ToolDto>> GetListAsync(ToolFilterDto input)
        {
            var query = Repository.WithDetails(x => x.ClientMachine).AsQueryable();

            if (input.Id.HasValue)
                query = query.Where(x => x.Id == input.Id.Value);

            if (!string.IsNullOrEmpty(input.Name))
                query = Repository.FullTextSearch(query, x => x.Name, input.Name);

            if (!string.IsNullOrEmpty(input.AppId))
                query = query.Where(x => x.AppId.ToString().Contains(input.AppId));

            if (!string.IsNullOrEmpty(input.Version))
                query = query.Where(x => x.Version.Contains(input.Version));

            DateTime currentDateTime = DateTime.UtcNow;
            var timeDetermine = currentDateTime.AddMinutes(-_toolInterval);
            if (input.ToolStatus.HasValue)
            {
                if (input.ToolStatus.Value == ToolStatus.Active)
                    query = query.Where(x => x.LastUpdate >= timeDetermine);

                if (input.ToolStatus.Value == ToolStatus.InActive)
                    query = query.Where(x => x.LastUpdate < timeDetermine);
            }

            if (input.ClientId.HasValue)
                query = query.Where(x => x.ClientId == input.ClientId.Value);

            if (!string.IsNullOrEmpty(input.Sorting))
                query = ApplySorting(query, input);
            else
                query = ApplyDefaultSorting(query);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                query = ApplyPaging(query, input);

            var userQuery = _userRepository.AsQueryable();

            var joinQuery = query.Join(
                userQuery,
                tool => tool.ClientMachine.UserId,
                user => user.Id,
                (tool, user) => new { tool, user });

            var count = await AsyncExecuter.CountAsync(joinQuery);
            var toolDto = joinQuery.Select(x => new ToolDto() 
            {
                Id = x.tool.Id,
                Name = x.tool.Name,
                AppId = x.tool.AppId,
                Version = x.tool.Version,
                ClientId = x.tool.ClientId,
                ExeFilePath = x.tool.ExeFilePath,
                ProcessId = x.tool.ProcessId,
                ProcessState = x.tool.ProcessState,
                LastUpdate = x.tool.LastUpdate,
                ToolStatus = x.tool.LastUpdate >= timeDetermine ? ToolStatus.Active : ToolStatus.InActive,
                Username = x.user.UserName,
                ClientMachine = ObjectMapper.Map<ClientMachine, ClientMachineDto>(x.tool.ClientMachine),
            }).ToList();

            return new PagedResultDto<ToolDto>(count, toolDto);
        }

        [Authorize(ToolManagerPermissions.Tools.Default)]
        public override Task<ToolDto> GetAsync(long id)
        {
            return base.GetAsync(id);
        }

        public async Task<ToolDto> CreateOrUpdateAsync(CreateUpdateToolDto input)
        {
            try
            {
                var tool = await AsyncExecuter
                .FirstOrDefaultAsync(Repository
                .WithDetails(x => x.ClientMachine)
                .Where(x => x.AppId == input.AppId &&
                    x.ClientMachine.IPLan == input.IPLan &&
                    x.ClientMachine.IPPublic == input.IPPublic));

                if (tool != null)
                {
                    tool.Name = input.Name;
                    tool.Version = input.Version;
                    tool.ExeFilePath = input.ExeFilePath;
                    tool.LastUpdate = input.LastUpdate;
                    tool.ProcessId = input.ProcessId;
                    tool.ProcessState = ProcessState.Started;
                    tool.SentMail = false;
                    tool.ClientMachine.Timestamp = CommonMethod.GetTimestamp();
                    tool = await Repository.UpdateAsync(tool, autoSave: true);

                    var toolDto = ObjectMapper.Map<Tool, ToolDto>(tool);
                    toolDto.ToolStatus = ToolStatus.Active;
                    toolDto.ClientMachine = null;
                    await _exchangeHub.Clients.All.ReceiveFromTool(toolDto);
                }
                else
                {
                    var clientMachine = await AsyncExecuter.FirstOrDefaultAsync(_clientMachineRepository
                        .Where(x => x.IPLan == input.IPLan &&
                            x.IPPublic == input.IPPublic &&
                            x.LastUpdate >= DateTime.UtcNow.AddSeconds(-_clientInterval)));
                    if (clientMachine != null)
                    {
                        clientMachine.Timestamp = CommonMethod.GetTimestamp();
                        await _clientMachineRepository.UpdateAsync(clientMachine, autoSave: true);
                        
                        var toolMapper = ObjectMapper.Map<CreateUpdateToolDto, Tool>(input);
                        toolMapper.ProcessState = ProcessState.Started;
                        toolMapper.ClientId = clientMachine.Id;
                        toolMapper.SentMail = false;
                        tool = await Repository.InsertAsync(toolMapper, autoSave: true);
                        await _exchangeHub.Clients.All.ReloadToolTable();
                    }
                }

                return ObjectMapper.Map<Tool, ToolDto>(tool);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [Authorize(ToolManagerPermissions.Tools.Delete)]
        public async Task DeleteAsync(long id)
        {
            await Repository.DeleteAsync(id);
        }

        //[Authorize(ToolManagerPermissions.Tools.Edit)]
        public async Task<ToolDto> UpdateStateAsync(long id, DateTime? lastUpdate, ProcessState processState, bool SentMail)
        {
            var query = Repository.Where(x => x.Id == id);
            var tool = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (lastUpdate.HasValue)
            {
                tool.LastUpdate = lastUpdate.Value;
            }
            tool.ProcessState = processState;
            tool.SentMail = SentMail;
            tool = await Repository.UpdateAsync(tool, autoSave: true);

            return ObjectMapper.Map<Tool, ToolDto>(tool);
        }

        public async Task<ToolReportDto> GetToolReportAsync()
        {
            var lastUpdates = await AsyncExecuter.ToListAsync(Repository.Select(x => x.LastUpdate));
            var report = new ToolReportDto()
            {
                ActiveNumber = lastUpdates.Count(x => x >= DateTime.UtcNow.AddMinutes(-_toolInterval)),
                InactiveNumber = lastUpdates.Count(x => x < DateTime.UtcNow.AddMinutes(-_toolInterval)),
            };
            return report;
        }

        public async Task UpdateProcessesAsync(List<ToolProcessDto> input)
        {
            var tool = ObjectMapper.Map<List<ToolProcessDto>, List<Tool>>(input);
            await Repository.BulkUpdateAsync(tool, 0);
        }

        public async Task<List<ToolProcessDto>> GetToolProcessesAsync(long clientMachineId)
        {
            var tool = await AsyncExecuter.ToListAsync(Repository.Where(x => x.ClientId == clientMachineId));
            return ObjectMapper.Map<List<Tool>, List<ToolProcessDto>>(tool);
        }
    }
}
