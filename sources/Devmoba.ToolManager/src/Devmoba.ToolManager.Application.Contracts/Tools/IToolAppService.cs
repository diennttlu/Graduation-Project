using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Devmoba.ToolManager.Tools
{
    public interface IToolAppService : IReadOnlyAppService<
        ToolDto, 
        long, 
        ToolFilterDto>, IDeleteAppService<long>
    {
        Task<ToolDto> CreateOrUpdateAsync(CreateUpdateToolDto input);

        Task<ToolDto> UpdateStateAsync(long id, DateTime? lastUpdate, ProcessState processState, bool SentMail);

        Task<ToolReportDto> GetToolReportAsync();

        Task UpdateProcessesAsync(List<ToolProcessDto> input);

        Task<List<ToolProcessDto>> GetToolProcessesAsync(long clientMachineId);
    }
}
