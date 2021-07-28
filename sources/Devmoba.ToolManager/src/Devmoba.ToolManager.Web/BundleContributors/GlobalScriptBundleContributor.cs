using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Devmoba.ToolManager.Web.BundleContributors
{
    public class GlobalScriptBundleContributor  : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            //context.Files.Clear();
            context.Files.Add("/libs/moment/moment.js");
            context.Files.Add("/libs/devmoba/core/devmoba.js"); 
            context.Files.Add("/libs/common/function.js"); 
        }
    }
}
