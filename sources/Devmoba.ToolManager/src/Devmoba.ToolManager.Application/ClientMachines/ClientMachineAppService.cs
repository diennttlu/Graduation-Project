using Devmoba.ToolManager.Localization;
using Devmoba.ToolManager.Permissions;
using Devmoba.ToolManager.Repositories;
using Devmoba.ToolManager.Tools;
using Devmoba.ToolManager.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Devmoba.ToolManager.ClientMachines
{
    [RemoteService(IsEnabled = false)]
    public class ClientMachineAppService : CrudAppService<
        ClientMachine,
        ClientMachineDto,
        long,
        ClientMachineFilterDto,
        CreateUpdateClientMachineDto,
        CreateUpdateClientMachineDto>, IClientMachineAppService
    {

        private new IClientMachineRepository Repository;
        private readonly IConfiguration _cfg;
        private readonly IRepository<AppUser, Guid> _userRepository;
        private readonly IStringLocalizer<ToolManagerResource> _stringLocalizer;
        private readonly int _toolInterval;
        private readonly int _clientInterval;

        public ClientMachineAppService(
           IClientMachineRepository repository,
            IRepository<AppUser, Guid> userRepository,
            IStringLocalizer<ToolManagerResource> stringLocalizer,
            IConfiguration cfg) 
            : base(repository)
        {
            Repository = repository;
            _cfg = cfg;
            _userRepository = userRepository;
            _stringLocalizer = stringLocalizer;
            _toolInterval = _cfg.GetValue<int>(CommonContants.ToolInterval);
            _clientInterval = _cfg.GetValue<int>(CommonContants.ClientMachineInterval);
        }

        #region CRUD

      
        [Authorize(ToolManagerPermissions.ClientMachines.Default)]
        public override async Task<PagedResultDto<ClientMachineDto>> GetListAsync(ClientMachineFilterDto input)
        {
            var query = Repository.AsQueryable();

            if (input.Id.HasValue)
                query = query.Where(x => x.Id == input.Id.Value);

            if (!string.IsNullOrEmpty(input.IpLan))
                query = query.Where(x => x.IPLan.Contains(input.IpLan));

            if (!string.IsNullOrEmpty(input.IpPublic))
                query = query.Where(x => x.IPPublic.Contains(input.IpPublic));

            if (!string.IsNullOrEmpty(input.Sorting))
                query = ApplySorting(query, input);
            else
                query = ApplyDefaultSorting(query);

            var interval = DateTime.UtcNow.AddSeconds(-_clientInterval);

            if (input.ClientStatus.HasValue)
            {
                if (input.ClientStatus.Value == ClientStatus.Online)
                    query = query.Where(x => x.LastUpdate >= interval);

                if (input.ClientStatus.Value == ClientStatus.Offline)
                    query = query.Where(x => x.LastUpdate < interval);
            }


            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                query = ApplyPaging(query, input);

            var userQuery = _userRepository.AsQueryable();
            if (!string.IsNullOrEmpty(input.Username))
                userQuery = userQuery.Where(x => x.UserName.Contains(input.Username));

            var joinQuery = query.Join(
                userQuery,
                client => client.UserId,
                user => user.Id,
                (client, user) => new {client, user});

            var count = await AsyncExecuter.CountAsync(joinQuery);
            var clientMachineDtos = joinQuery.Select(x => new ClientMachineDto()
            {
                Id = x.client.Id,
                IPLan = x.client.IPLan,
                IPPublic = x.client.IPPublic,
                LastUpdate = x.client.LastUpdate,
                ClientStatus = x.client.LastUpdate >= interval ? ClientStatus.Online : ClientStatus.Offline,
                Username = x.user.UserName,
                UserId = x.user.Id,
                Tools = ObjectMapper.Map<ICollection<Tool>, ICollection<ToolDto>>(x.client.Tools),
            }).ToList();

            return new PagedResultDto<ClientMachineDto>(count, clientMachineDtos);
        }

        [Authorize(ToolManagerPermissions.ClientMachines.Default)]
        public override Task<ClientMachineDto> GetAsync(long id)
        {
            return base.GetAsync(id);
        }

        [Authorize(ToolManagerPermissions.ClientMachines.Create)]
        public override Task<ClientMachineDto> CreateAsync(CreateUpdateClientMachineDto input)
        {
            return base.CreateAsync(input);
        }

        [Authorize(ToolManagerPermissions.ClientMachines.Edit)]
        public override Task<ClientMachineDto> UpdateAsync(long id, CreateUpdateClientMachineDto input)
        {
            return base.UpdateAsync(id, input);
        }

        [Authorize(ToolManagerPermissions.ClientMachines.Delete)]
        public override async Task DeleteAsync(long id)
        {
            var clientMachine = Repository
                .WithDetails(x => x.Tools)
                .Where(x => x.Id == id).FirstOrDefault();
            if (clientMachine.Tools.Count > 0)
            {
                throw new UserFriendlyException(_stringLocalizer["HasTool"]);
            }
            await Repository.DeleteAsync(id);
        }
        #endregion

        public async Task<ClientMachineDto> GetByUserIdAsync(Guid userId)
        {
            var query = Repository.Where(x => x.UserId == userId);
            var clientMachine = await AsyncExecuter.FirstOrDefaultAsync(query);

            return ObjectMapper.Map<ClientMachine, ClientMachineDto>(clientMachine);
        }

        public async Task<List<Guid>> GetListUserIdOnlineAsync()
        {
            var query = Repository
                .Where(x => x.LastUpdate >= DateTime.UtcNow.AddSeconds(-_clientInterval))
                .Select(x => x.UserId);

            return await AsyncExecuter.ToListAsync(query);
        }

        public async Task<List<ClientMachineSelectDto>> GetAllAsSelectionsAsync()
        {
            var userQuery = _userRepository.AsQueryable();
            var query = Repository.AsQueryable();
            var joinQuery = query.Join(
               userQuery,
               client => client.UserId,
               user => user.Id,
               (client, user) => new { client, user });
            var result = joinQuery.Select(x => new ClientMachineSelectDto()
            {
                Id = x.client.Id,
                Username = x.user.UserName
            });
            return await AsyncExecuter.ToListAsync(result);
        }

        public async Task<List<ClientMachineReportEmailDto>> GetClientMachineReportEmailsAsync()
        {
            var query = Repository.WithDetails(x => x.Tools)
                .Where(x => x.Tools.Where(x => x.LastUpdate < DateTime.UtcNow.AddMinutes(-_toolInterval) && x.SentMail == false).Any())
                .Select(x => new ClientMachineReportEmailDto()
                {
                    Id = x.Id,
                    IPLan = x.IPLan,
                    IPPublic = x.IPPublic,
                    ToolInactives = ObjectMapper.Map<List<Tool>, List<ToolDto>>(x.Tools
                        .Where(x => x.LastUpdate < DateTime.UtcNow.AddMinutes(-_toolInterval) && x.SentMail == false)
                        .ToList()),
                    TotalTool = x.Tools.Count
                });
            return await AsyncExecuter.ToListAsync(query);
        }

        public async Task<ClientMachineReportDto> GetClientMachineReportAsync()
        {
            var lastUpdates = await AsyncExecuter.ToListAsync(Repository.Select(x => x.LastUpdate));

            var report = new ClientMachineReportDto()
            { 
                OfflineNumber = lastUpdates.Count(x => x < DateTime.UtcNow.AddSeconds(-_clientInterval)),
                OnlineNumber = lastUpdates.Count(x => x >= DateTime.UtcNow.AddSeconds(-_clientInterval)),
            };
            return report;
        }

        [Authorize(ToolManagerPermissions.Dashboards.Default)]
        public async Task<PagedResultDto<ClientMachineOverviewDto>> GetClientMachineOverviewAsync()
        {
            var userQuery = _userRepository.AsQueryable();
            var query = Repository.AsQueryable();
            var joinQuery = query.Join(
               userQuery,
               client => client.UserId,
               user => user.Id,
               (client, user) => new { client, user });
            var count = await AsyncExecuter.CountAsync(joinQuery);
            var result = joinQuery.Select(x => new ClientMachineOverviewDto()
            {
                ClientName = x.user.UserName,
                IPLan = x.client.IPLan,
                IPPublic = x.client.IPPublic,
                ClientStatus = x.client.LastUpdate >= DateTime.UtcNow.AddSeconds(-_clientInterval) ? ClientStatus.Online : ClientStatus.Offline,
                ToolReport = new ToolReportDto()
                {
                    ActiveNumber = x.client.Tools.Count(x => x.LastUpdate >= DateTime.UtcNow.AddMinutes(-_toolInterval)),
                    InactiveNumber = x.client.Tools.Count(x => x.LastUpdate < DateTime.UtcNow.AddMinutes(-_toolInterval))
                }
            }).ToList();

            return new PagedResultDto<ClientMachineOverviewDto>(count, result);
        }

        public Task<ClientMachineDto> UpdateStatusAsync(Guid userId, ClientStatus status)
        {
            var clientMachine = Repository.Where(x => x.UserId == userId).FirstOrDefault();
            if (clientMachine != null)
            {
                switch (status)
                {
                    case ClientStatus.Offline:
                        clientMachine.LastUpdate = DateTime.UtcNow.AddSeconds(-_clientInterval);
                        break;
                    case ClientStatus.Online:
                        clientMachine.LastUpdate = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
                return Task.FromResult(ObjectMapper.Map<ClientMachine, ClientMachineDto>(clientMachine));
               
            }
            return Task.FromResult(new ClientMachineDto());
        }
    }
}
