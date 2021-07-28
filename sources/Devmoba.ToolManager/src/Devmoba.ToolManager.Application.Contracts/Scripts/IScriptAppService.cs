using System.Collections.Generic;
using Volo.Abp.Application.Services;

namespace Devmoba.ToolManager.Scripts
{
    public interface IScriptAppService : ICrudAppService<
        ScriptDto,
        long,
        ScriptFilterDto,
        CreateUpdateScriptDto,
        CreateUpdateScriptDto>
    {
        List<ScriptSelectionDto> GetAllAsSelections();
    }
}
