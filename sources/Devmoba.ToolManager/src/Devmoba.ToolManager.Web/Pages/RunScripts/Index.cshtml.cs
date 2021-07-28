using Devmoba.ToolManager.ClientMachines;
using Devmoba.ToolManager.Scripts;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace Devmoba.ToolManager.Web.Pages.RunScripts
{
    public class IndexModel : ToolManagerPageModel
    {
        private readonly IScriptAppService _scriptAppService;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IClientMachineAppService _clientMachineAppService;

        public IndexModel(
            IScriptAppService scriptAppService,
            IIdentityUserRepository identityUserRepository,
            IClientMachineAppService clientMachineAppService)
        {
            _scriptAppService = scriptAppService;
            _identityUserRepository = identityUserRepository;
            _clientMachineAppService = clientMachineAppService;
        }

        public async Task OnGetAsync()
        {
            var scriptSelections = _scriptAppService.GetAllAsSelections();
            var users = await _identityUserRepository.GetListAsync();
            var userIds = await _clientMachineAppService.GetListUserIdOnlineAsync();

            var usernames = users.Join(userIds,
                user => user.Id,
                id => id,
                (user, id) => new
                {
                    Username = user.UserName,
                    HasChecked = false
                }).ToList();

            ViewData.Add("allScripts", SerializeObject(scriptSelections));
            ViewData.Add("clientUserNames", SerializeObject(usernames));
        }
    }
}