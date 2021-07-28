using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace Devmoba.ToolManager.Web
{
    [Dependency(ReplaceServices = true)]
    public class ToolManagerBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "ToolManager";
    }
}
