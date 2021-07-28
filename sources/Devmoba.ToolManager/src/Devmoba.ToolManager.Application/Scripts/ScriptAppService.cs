using Devmoba.ToolManager.Localization;
using Devmoba.ToolManager.Permissions;
using Devmoba.ToolManager.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Devmoba.ToolManager.Scripts
{
    [RemoteService(IsEnabled = false)]
    public class ScriptAppService : CrudAppService<
        Script,
        ScriptDto,
        long,
        ScriptFilterDto,
        CreateUpdateScriptDto,
        CreateUpdateScriptDto>, IScriptAppService
    {
        private new IScriptRepository Repository;
        private readonly IDependencyRepository _dependencyRepository;
        private readonly IStringLocalizer<ToolManagerResource> _stringLocalizer;

        public ScriptAppService(
            IScriptRepository repository,
            IDependencyRepository dependencyRepository,
            IStringLocalizer<ToolManagerResource> stringLocalizer) : base(repository)
        {
            Repository = repository;
            _dependencyRepository = dependencyRepository;
            _stringLocalizer = stringLocalizer;
        }

        [Authorize(ToolManagerPermissions.Scripts.Default)]
        public override async Task<PagedResultDto<ScriptDto>> GetListAsync(ScriptFilterDto input)
        {
            var query = Repository.WithDetails(x => x.Dependencies).AsQueryable();

            if (input.Id.HasValue)
                query = query.Where(s => s.Id == input.Id.Value);

            if (!string.IsNullOrEmpty(input.Name))
                query = Repository.FullTextSearch(query, s => s.Name, input.Name);

            var count = await AsyncExecuter.CountAsync(query);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                query = ApplyPaging(query, input);

            if (!string.IsNullOrEmpty(input.Sorting))
                query = ApplySorting(query, input);
            else
                query = ApplyDefaultSorting(query);

            var scripts = await AsyncExecuter.ToListAsync(query);
            var scriptDtos = ObjectMapper.Map<List<Script>, List<ScriptDto>>(scripts);
           
            return new PagedResultDto<ScriptDto>(count, scriptDtos);
        }

        [Authorize(ToolManagerPermissions.Scripts.Default)]
        public async override Task<ScriptDto> GetAsync(long id)
        {
            var query = Repository.WithDetails(x => x.Dependencies).Where(x => x.Id == id);
            var script = await AsyncExecuter.FirstOrDefaultAsync(query);
            return ObjectMapper.Map<Script, ScriptDto>(script);
        }

        [Authorize(ToolManagerPermissions.Scripts.Create)]
        public override async Task<ScriptDto> CreateAsync(CreateUpdateScriptDto input)
        {
            input.Comment = CommentSplit.GetComment(input.Content);
            var script = await Repository.InsertAsync(ObjectMapper.Map<CreateUpdateScriptDto, Script>(input));
            if (input.DependencyIds != null && input.DependencyIds.Count > 0)
            {
                script.Dependencies = new List<Dependency>();
                foreach (var item in input.DependencyIds)
                {
                    script.Dependencies.Add(new Dependency() 
                    {
                        ScriptId = script.Id,
                        ScriptDependencyId = item
                    });
                }
            }
            return ObjectMapper.Map<Script, ScriptDto>(script);
        }

        [Authorize(ToolManagerPermissions.Scripts.Edit)]
        public override async Task<ScriptDto> UpdateAsync(long id, CreateUpdateScriptDto input)
        {
            var query = Repository.WithDetails(x => x.Dependencies).Where(x => x.Id == id);
            var script = await AsyncExecuter.FirstOrDefaultAsync(query);
            script.Name = input.Name;
            script.Comment = CommentSplit.GetComment(input.Content);
            script.Content = input.Content;

            foreach (var dependencyChosen in input.DependencyChosens)
            {
                if (dependencyChosen.Checked && !dependencyChosen.InDb)
                {
                    script.Dependencies.Add(new Dependency()
                    {
                        ScriptId = script.Id,
                        ScriptDependencyId = dependencyChosen.ScriptDependencyId
                    });
                } 
                else if (!dependencyChosen.Checked && dependencyChosen.InDb) {
                    var dependency = _dependencyRepository
                        .Where(x => x.ScriptId == script.Id && x.ScriptDependencyId == dependencyChosen.ScriptDependencyId)
                        .FirstOrDefault();
                    script.Dependencies.Remove(dependency);
                }
            }

            return ObjectMapper.Map<Script, ScriptDto>(script);
        }

        [Authorize(ToolManagerPermissions.Scripts.Delete)]
        public override async Task DeleteAsync(long id)
        {
            var script = Repository
                .WithDetails(x => x.ScriptDependencies)
                .Where(x => x.Id == id).FirstOrDefault();
            if (script.ScriptDependencies.Count > 0)
            {
                throw new UserFriendlyException(_stringLocalizer["HasDependency"]);
            }

            await Repository.DeleteAsync(id);
        }

        public List<ScriptSelectionDto> GetAllAsSelections()
        {
            var scriptSelections = Repository.ToList();
            return ObjectMapper.Map<List<Script>, List<ScriptSelectionDto>>(scriptSelections);
        }
    }
}
