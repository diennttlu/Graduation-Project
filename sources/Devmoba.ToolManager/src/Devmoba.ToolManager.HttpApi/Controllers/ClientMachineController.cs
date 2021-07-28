using Devmoba.ToolManager.ClientMachines;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Devmoba.ToolManager.Controllers
{
    [RemoteService(Name = ToolManagerHttpApiModule.RemoteServiceName)]
    [Route("/api/clientMachines")]
    public class ClientMachineController : AbpController, IClientMachineAppService
    {
        private readonly IClientMachineAppService _clientMachineAppService;

        public ClientMachineController(IClientMachineAppService clientMachineAppService)
        {
            _clientMachineAppService = clientMachineAppService;
        }

        [RemoteService(IsEnabled = false)]
        [HttpGet]
        [Route("{id}")]
        public async Task<ClientMachineDto> GetAsync(long id)
        {
            return await _clientMachineAppService.GetAsync(id);
        }

        [HttpGet]
        public async Task<PagedResultDto<ClientMachineDto>> GetListAsync(ClientMachineFilterDto input)
        {
            return await _clientMachineAppService.GetListAsync(input);
        }

        [RemoteService(IsEnabled = false)]
        [HttpPost]
        public async Task<ClientMachineDto> CreateAsync(CreateUpdateClientMachineDto input)
        {
            return await _clientMachineAppService.CreateAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(long id)
        {
            await _clientMachineAppService.DeleteAsync(id);
        }

        [RemoteService(IsEnabled = false)]
        [HttpPut]
        [Route("{id}")]
        public async Task<ClientMachineDto> UpdateAsync(long id, CreateUpdateClientMachineDto input)
        {
            return await _clientMachineAppService.UpdateAsync(id, input);
        }

        [HttpGet]
        [Route("overview")]
        public async Task<PagedResultDto<ClientMachineOverviewDto>> GetClientMachineOverviewAsync()
        {
            return await _clientMachineAppService.GetClientMachineOverviewAsync();
        }

        #region Remote Service

        [RemoteService(IsEnabled = false)]
        public Task<ClientMachineDto> GetByIpAddress(string ipLan, string ipPublic)
        {
            throw new NotImplementedException();
        }

        [RemoteService(IsEnabled = false)]
        public Task<ClientMachineDto> GetByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        [RemoteService(IsEnabled = false)]
        public Task<List<Guid>> GetListUserIdOnlineAsync()
        {
            throw new NotImplementedException();
        }

        [RemoteService(IsEnabled = false)]
        public Task<List<ClientMachineSelectDto>> GetAllAsSelectionsAsync()
        {
            throw new NotImplementedException();
        }

        [RemoteService(IsEnabled = false)]
        public Task<List<ClientMachineReportEmailDto>> GetClientMachineReportEmailsAsync()
        {
            throw new NotImplementedException();
        }

        [RemoteService(IsEnabled = false)]
        public Task<ClientMachineReportDto> GetClientMachineReportAsync()
        {
            throw new NotImplementedException();
        }

        [RemoteService(IsEnabled = false)]
        public Task<ClientMachineDto> UpdateStatusAsync(Guid userId, ClientStatus status)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
