using System;
using System.Collections.Generic;
using System.Text;
using Devmoba.ToolManager.Localization;
using Volo.Abp.Application.Services;

namespace Devmoba.ToolManager
{
    /* Inherit your application services from this class.
     */
    public abstract class ToolManagerAppService : ApplicationService
    {
        protected ToolManagerAppService()
        {
            LocalizationResource = typeof(ToolManagerResource);
        }
    }
}
