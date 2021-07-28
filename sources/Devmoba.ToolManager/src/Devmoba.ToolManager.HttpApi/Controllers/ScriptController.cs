using Devmoba.ToolManager.Scripts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Devmoba.ToolManager.Controllers
{
    [RemoteService(Name = ToolManagerHttpApiModule.RemoteServiceName)]
    [Route("/api/scripts")]
    public class ScriptController : AbpController, IScriptAppService
    {
        private readonly IScriptAppService _scriptAppService;

        public ScriptController(IScriptAppService scriptAppService)
        {
            _scriptAppService = scriptAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ScriptDto> GetAsync(long id)
        {
            return await _scriptAppService.GetAsync(id);
        }

        [HttpGet]
        public async Task<PagedResultDto<ScriptDto>> GetListAsync(ScriptFilterDto input)
        {
            return await _scriptAppService.GetListAsync(input);
        }

        [HttpPost]
        public async Task<ScriptDto> CreateAsync(CreateUpdateScriptDto input)
        {
            return await _scriptAppService.CreateAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(long id)
        {
            await _scriptAppService.DeleteAsync(id);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ScriptDto> UpdateAsync(long id, CreateUpdateScriptDto input)
        {
            return await _scriptAppService.UpdateAsync(id, input);
        }

        [RemoteService(IsEnabled = false)]
        public List<ScriptSelectionDto> GetAllAsSelections()
        {
            throw new NotImplementedException();
        }
    }
}
