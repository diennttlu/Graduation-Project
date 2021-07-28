using Devmoba.ToolManager.Dependencies;
using Devmoba.ToolManager.Scripts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Devmoba.ToolManager.Web.Pages.Scripts
{
    public class EditModel : ToolManagerPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        private readonly IScriptAppService _scriptAppService;
        private readonly IDependencyAppService _dependencyAppService;

        public EditModel(IScriptAppService scriptAppService, IDependencyAppService dependencyAppService)
        {
            _scriptAppService = scriptAppService;
            _dependencyAppService = dependencyAppService;
          
        }

        public async Task OnGetAsync()
        {
            var script = await _scriptAppService.GetAsync(Id);
            var scriptSelections = _scriptAppService.GetAllAsSelections().Where(x => x.Id != script.Id).ToList();
            var dependencies = await _dependencyAppService.GetAllSelection();
            
            if (script.Dependencies != null)
            {
                foreach (var item in scriptSelections)
                {
                    if (dependencies.Where(x => x.ScriptId == script.Id && x.ScriptDependencyId == item.Id).Any())
                        item.Checked = true;
                }
            }
            ViewData.Add("allScripts", SerializeObject(scriptSelections));
            ViewData.Add("script", SerializeObject(ObjectMapper.Map<ScriptDto, CreateUpdateScriptDto>(script)));
        }

        
    }
}