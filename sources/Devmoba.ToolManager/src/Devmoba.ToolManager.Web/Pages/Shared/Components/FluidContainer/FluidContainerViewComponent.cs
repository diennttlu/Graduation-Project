using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Devmoba.ToolManager.Web.Pages.Shared.Components.FluidContainer
{
    public class FluidContainerViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("/Pages/Shared/Components/FluidContainer/Default.cshtml");
        }
    }
}
