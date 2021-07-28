using Devmoba.ToolManager.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Devmoba.ToolManager.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class ToolManagerController : AbpController
    {
        protected ToolManagerController()
        {
            LocalizationResource = typeof(ToolManagerResource);
        }
    }
}