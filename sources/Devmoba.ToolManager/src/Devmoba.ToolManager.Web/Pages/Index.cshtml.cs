using Devmoba.ToolManager.ClientMachines;
using Devmoba.ToolManager.Tools;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Devmoba.ToolManager.Web.Pages
{
    public class IndexModel : ToolManagerPageModel
    {
        //private readonly IClientMachineAppService _clientMachineAppService;
        //private readonly IToolAppService _toolAppService;

        //public ClientMachineReportDto ClientReport { get; set; }
        
        //public ToolReportDto ToolReport { get; set; }

        //public IndexModel(
        //    IClientMachineAppService clientMachineAppService, 
        //    IToolAppService toolAppService)
        //{
        //    _clientMachineAppService = clientMachineAppService;
        //    _toolAppService = toolAppService;
        //}

        public async Task OnGetAsync()
        {

            //ClientReport = await _clientMachineAppService.GetClientMachineReportAsync();
            //ToolReport = await _toolAppService.GetToolReportAsync();
            //var allClientStatus = Enum.GetValues(typeof(ClientStatus)).Cast<ClientStatus>()
            //  .Select(item => new SelectListItem()
            //  {
            //      Text = item.ToString(),
            //      Value = item.ToString()
            //  }).ToList();

            //ViewData.Add("allClientStatus", SerializeObject(allClientStatus));
        }

    }

}