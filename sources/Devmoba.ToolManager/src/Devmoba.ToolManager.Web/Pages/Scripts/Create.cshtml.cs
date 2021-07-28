using Devmoba.ToolManager.Scripts;

namespace Devmoba.ToolManager.Web.Pages.Scripts
{
    public class CreateModel : ToolManagerPageModel
    {
        private readonly IScriptAppService _scriptAppService;

        public CreateModel(IScriptAppService scriptAppService)
        {
            _scriptAppService = scriptAppService;
        }
        public void OnGet()
        {
            var scriptSelections = _scriptAppService.GetAllAsSelections();
            ViewData.Add("allScripts", SerializeObject(scriptSelections));
        }
    }
}