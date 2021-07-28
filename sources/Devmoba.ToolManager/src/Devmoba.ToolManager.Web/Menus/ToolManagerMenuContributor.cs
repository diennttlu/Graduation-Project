using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Devmoba.ToolManager.Localization;
using Devmoba.ToolManager.MultiTenancy;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Devmoba.ToolManager.Permissions;

namespace Devmoba.ToolManager.Web.Menus
{
    public class ToolManagerMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var administration = context.Menu.GetAdministration();
            var l = context.GetLocalizer<ToolManagerResource>();
            if (!MultiTenancyConsts.IsEnabled)
            {
                administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
            }
            //if (await context.IsGrantedAsync(ToolManagerPermissions.Dashboards.Default))
            //{
            //    context.Menu.AddItem(
            //        new ApplicationMenuItem(
            //            "ToolManager",
            //            l["Menu:Dashboard"],
            //            "/Index"));
            //}
            if (await context.IsGrantedAsync(ToolManagerPermissions.ClientMachines.Default))
            {
                context.Menu.AddItem(
                    new ApplicationMenuItem(
                        "ToolManager",
                        l["Menu:ClientMachine"],
                        "/ClientMachines"));
            }
            if (await context.IsGrantedAsync(ToolManagerPermissions.Tools.Default))
            {
                context.Menu.AddItem(
                   new ApplicationMenuItem(
                       "ToolManager",
                       l["Menu:Tool"],
                       "/Tools"));
            }
            if (await context.IsGrantedAsync(ToolManagerPermissions.Scripts.Default))
            {
                context.Menu.AddItem(
                    new ApplicationMenuItem(
                        "ToolManager",
                        l["Menu:Script"],
                        "/Scripts"));
            }
            if (await context.IsGrantedAsync(ToolManagerPermissions.Scripts.RunScript))
            {
                context.Menu.AddItem(
                    new ApplicationMenuItem(
                        "ToolManager",
                        l["Menu:RunScript"],
                        "/RunScripts"));
            }
            
        }
    }
}
