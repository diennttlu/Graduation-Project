using Devmoba.ToolManager.Dependencies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Devmoba.ToolManager.Controllers
{
    [RemoteService(Name = ToolManagerHttpApiModule.RemoteServiceName)]
    [Route("/api/dependencies")]
    public class DependencyController : AbpController, IDependencyAppService
    {
        private readonly IDependencyAppService _dependencyAppService;

        public DependencyController(IDependencyAppService dependencyAppService)
        {
            _dependencyAppService = dependencyAppService;
        }

        [RemoteService(IsEnabled = false)]
        [HttpPost]
        public async Task<DependencyDto> CreateAsync(CreateUpdateDependencyDto input)
        {
            return await _dependencyAppService.CreateAsync(input);
        }

        [RemoteService(IsEnabled = false)]
        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(long id)
        {
            await _dependencyAppService.DeleteAsync(id);
        }

        //[HttpDelete]
        //[Route("{scriptId}/{scriptDependencyId}")]
        //public async Task DeleteAsync(long scriptId, long scriptDependencyId)
        //{
        //    await _dependencyAppService.DeleteAsync(scriptId, scriptDependencyId);
        //}

        [RemoteService(IsEnabled = false)]
        [HttpGet]
        public async Task<List<DependencyDto>> GetAllSelection()
        {
            return await _dependencyAppService.GetAllSelection();
        }

        [RemoteService(IsEnabled = false)]
        [HttpGet]
        [Route("{id}")]
        public async Task<DependencyDto> GetAsync(long id)
        {
            return await _dependencyAppService.GetAsync(id);
        }

        [RemoteService(IsEnabled = false)]
        public Task<PagedResultDto<DependencyDto>> GetListAsync(DependencyFilterDto input)
        {
            throw new System.NotImplementedException();
        }

        [RemoteService(IsEnabled = false)]
        public List<long> GetScriptDependencyIdByScriptId(long scriptId)
        {
            throw new System.NotImplementedException();
        }

        [RemoteService(IsEnabled = false)]
        public Task<DependencyDto> UpdateAsync(long id, CreateUpdateDependencyDto input)
        {
            throw new System.NotImplementedException();
        }
    }
}
