using System;
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

        Task<ToolDto> UpdateAsync(long id, DateTime? lastUpdate, bool SentMail);

        Task<ToolReportDto> GetToolReportAsync();

    }
}
