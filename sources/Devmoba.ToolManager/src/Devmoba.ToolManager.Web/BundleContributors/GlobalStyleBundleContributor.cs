using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Devmoba.ToolManager.Web.BundleContributors
{
    public class GlobalStyleBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            //context.Files.Clear();
            context.Files.Add("/styles/bs-card.css");
        }
    }
}
