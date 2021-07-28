using Devmoba.ToolManager.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Devmoba.ToolManager.Permissions
{
    public class ToolManagerPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var scriptGroup = context.AddGroup(ToolManagerPermissions.ScriptGroup, L("Permission:ScriptGroup"));
            var scriptManagement = scriptGroup.AddPermission(ToolManagerPermissions.Scripts.Default, L("Permission:Scripts"));

            scriptManagement.AddChild(ToolManagerPermissions.Scripts.RunScript, L("Permission:Scripts.RunScript"));
            scriptManagement.AddChild(ToolManagerPermissions.Scripts.Create, L("Permission:Scripts.Create"));
            scriptManagement.AddChild(ToolManagerPermissions.Scripts.Edit, L("Permission:Scripts.Edit"));
            scriptManagement.AddChild(ToolManagerPermissions.Scripts.Delete, L("Permission:Scripts.Delete"));

            var clientMachineGroup = context.AddGroup(ToolManagerPermissions.ClientMachineGroup, L("Permission:ClientMachineGroup"));
            var clientMachineManagement = clientMachineGroup.AddPermission(ToolManagerPermissions.ClientMachines.Default, L("Permission:ClientMachines"));

            clientMachineManagement.AddChild(ToolManagerPermissions.ClientMachines.Create, L("Permission:ClientMachines.Create"));
            clientMachineManagement.AddChild(ToolManagerPermissions.ClientMachines.Edit, L("Permission:ClientMachines.Edit"));
            clientMachineManagement.AddChild(ToolManagerPermissions.ClientMachines.Delete, L("Permission:ClientMachines.Delete"));

            var toolGroup = context.AddGroup(ToolManagerPermissions.ToolGroup, L("Permission:ToolGroup"));
            var toolManagement = toolGroup.AddPermission(ToolManagerPermissions.Tools.Default, L("Permission:Tools"));

            //toolManagement.AddChild(ToolManagerPermissions.Tools.Create, L("Permission:Tools.Create"));
            toolManagement.AddChild(ToolManagerPermissions.Tools.Edit, L("Permission:Tools.Edit"));
            toolManagement.AddChild(ToolManagerPermissions.Tools.Delete, L("Permission:Tools.Delete"));
            toolManagement.AddChild(ToolManagerPermissions.Tools.TurnOn, L("Permission:Tools.TurnOn"));
            toolManagement.AddChild(ToolManagerPermissions.Tools.TurnOff, L("Permission:Tools.TurnOff"));

            var dashboardGroup = context.AddGroup(ToolManagerPermissions.DashboardGroup, L("Permission:DashboardGroup"));
            var dashboardManagement = dashboardGroup.AddPermission(ToolManagerPermissions.Dashboards.Default, L("Permission:Dashboard"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ToolManagerResource>(name);
        }
    }
}
