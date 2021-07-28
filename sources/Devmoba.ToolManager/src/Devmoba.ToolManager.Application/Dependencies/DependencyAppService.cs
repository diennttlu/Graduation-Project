using Devmoba.ToolManager.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Devmoba.ToolManager.Dependencies
{
    [RemoteService(IsEnabled = false)]
    public class DependencyAppService : CrudAppService<
        Dependency,
        DependencyDto,
        long,
        DependencyFilterDto,
        CreateUpdateDependencyDto,
        CreateUpdateDependencyDto>, IDependencyAppService
    {
        private new IDependencyRepository Repository;
        public DependencyAppService(IDependencyRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<List<DependencyDto>> GetAllSelection()
        {
            var query = Repository.WithDetails(x => x.Script).AsQueryable();
            var dependencies = await AsyncExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<Dependency>, List<DependencyDto>>(dependencies);
        }

        public override Task DeleteAsync(long id)
        {
            return base.DeleteAsync(id);
        }

        public List<long> GetScriptDependencyIdByScriptId(long scriptId)
        {
            var scriptDependencyIds = Repository
                        .Where(x => x.ScriptId == scriptId)
                        .Select(x => x.ScriptDependencyId)
                        .ToList();
            return scriptDependencyIds;
        }
    }
}
