using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Devmoba.ToolManager.Web.Pages.ClientMachines
{
    public class IndexModel : ToolManagerPageModel
    {
        public IndexModel() {}

        public void OnGet()
        {
            var allClientStatus = Enum.GetValues(typeof(ClientStatus)).Cast<ClientStatus>()
              .Select(item => new SelectListItem()
              {
                  Text = item.ToString(),
                  Value = $"{(int)item}"
              }).ToList();

            ViewData.Add("allClientStatus", SerializeObject(allClientStatus));
        }
    }
}