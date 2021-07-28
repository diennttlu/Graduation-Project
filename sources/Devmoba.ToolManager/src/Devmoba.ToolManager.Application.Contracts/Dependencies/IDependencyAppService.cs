using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Devmoba.ToolManager.Dependencies
{
    public interface IDependencyAppService : ICrudAppService<
        DependencyDto, 
        long,
        DependencyFilterDto,
        CreateUpdateDependencyDto, 
        CreateUpdateDependencyDto>
    {
        Task<List<DependencyDto>> GetAllSelection();

        //Task DeleteAsync(long scriptId, long scriptDependencyId);

        List<long> GetScriptDependencyIdByScriptId(long scriptId);
    }
}
