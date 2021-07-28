using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Devmoba.ToolManager.ClientMachines
{
    public interface IClientMachineAppService : ICrudAppService<
        ClientMachineDto,
        long,
        ClientMachineFilterDto,
        CreateUpdateClientMachineDto,
        CreateUpdateClientMachineDto>
    {

        Task<ClientMachineDto> GetByUserIdAsync(Guid userId);

        Task<List<Guid>> GetListUserIdOnlineAsync();

        Task<List<ClientMachineSelectDto>> GetAllAsSelectionsAsync();

        Task<List<ClientMachineReportEmailDto>> GetClientMachineReportEmailsAsync();

        Task<ClientMachineReportDto> GetClientMachineReportAsync();

        Task<PagedResultDto<ClientMachineOverviewDto>> GetClientMachineOverviewAsync();

        Task<ClientMachineDto> UpdateStatusAsync(Guid userId, ClientStatus status);

    }
}
