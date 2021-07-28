using Devmoba.ToolManager.ClientMachines;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Devmoba.ToolManager.Web.Pages.Tools
{
    public class IndexModel : ToolManagerPageModel
    {
        private readonly IClientMachineAppService _clientMachineAppService;

        public IndexModel(IClientMachineAppService clientMachineAppService)
        {
            _clientMachineAppService = clientMachineAppService;
        }

        public async Task OnGetAsync()
        {
            var allToolStatus = Enum.GetValues(typeof(ToolStatus)).Cast<ToolStatus>()
              .Select(item => new SelectListItem()
              {
                  Text = item.ToString(),
                  Value = $"{(int)item}"
              }).ToList();
            var clientMachines = await _clientMachineAppService.GetAllAsSelectionsAsync();
            var allClientMachines = clientMachines.Select(item => new SelectListItem() 
            {
                Text = item.Username,
                Value = $"{(int)item.Id}"
            });

            ViewData.Add("allToolStatus", SerializeObject(allToolStatus));
            ViewData.Add("allClientMachines", SerializeObject(allClientMachines));
        }
    }
}