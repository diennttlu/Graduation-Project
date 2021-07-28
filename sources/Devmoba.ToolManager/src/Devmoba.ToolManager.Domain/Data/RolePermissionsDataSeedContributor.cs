using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace Devmoba.ToolManager.Data
{
    public class RolePermissionsDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IdentityRoleManager _roleManager;
        private readonly IPermissionManager _permissionManager;

        public RolePermissionsDataSeedContributor(IdentityRoleManager roleManager, IPermissionManager permissionManager)
        {
            _roleManager = roleManager;
            _permissionManager = permissionManager;
        }

        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedRolesAsync();
            await SeedPermissionsAsync();
        }

        private async Task SeedRolesAsync()
        {
            var clientRole = new IdentityRole(Guid.NewGuid(), CommonContants.RoleNameClient, null)
            {
                IsDefault = false,
                IsPublic = false,
                IsStatic = true
            };

            var memberRole = new IdentityRole(Guid.NewGuid(), CommonContants.RoleNameMember, null)
            {
                IsDefault = false,
                IsPublic = false,
                IsStatic = true
            };

            await _roleManager.CreateAsync(clientRole);
            await _roleManager.CreateAsync(memberRole);
        }

        private async Task SeedPermissionsAsync()
        {
            #region client's permissions
            await _permissionManager.SetForRoleAsync(CommonContants.RoleNameClient, PermissionNames.ScriptGroup_Default, isGranted: false);
            await _permissionManager.SetForRoleAsync(CommonContants.RoleNameClient, PermissionNames.ScriptGroup_RunScript, isGranted: false);
            await _permissionManager.SetForRoleAsync(CommonContants.RoleNameClient, PermissionNames.ScriptGroup_Create, isGranted: false);
            await _permissionManager.SetForRoleAsync(CommonContants.RoleNameClient, PermissionNames.ScriptGroup_Edit, isGranted: false);
            await _permissionManager.SetForRoleAsync(CommonContants.RoleNameClient, PermissionNames.ScriptGroup_Delete, isGranted: false);

            await _permissionManager.SetForRoleAsync(CommonContants.RoleNameClient, PermissionNames.ClientMachineGroup_Default, isGranted: false);
            await _permissionManager.SetForRoleAsync(CommonContants.RoleNameClient, PermissionNames.ClientMachineGroup_Create, isGranted: true);
            await _permissionManager.SetForRoleAsync(CommonContants.RoleNameClient, PermissionNames.ClientMachineGroup_Edit, isGranted: true);
            await _permissionManager.SetForRoleAsync(CommonContants.RoleNameClient, PermissionNames.ClientMachineGroup_Default, isGranted: false);

            await _permissionManager.SetForRoleAsync(CommonContants.RoleNameClient, PermissionNames.ToolGroup_Default, isGranted: false);
            //await _permissionManager.SetForRoleAsync(CommomContants.RoleNameClient, PermissionNames.ToolGroup_Create, isGranted: false);
            await _permissionManager.SetForRoleAsync(CommonContants.RoleNameClient, PermissionNames.ToolGroup_Edit, isGranted: true);
            await _permissionManager.SetForRoleAsync(CommonContants.RoleNameClient, PermissionNames.ToolGroup_Delete, isGranted: false);
            await _permissionManager.SetForRoleAsync(CommonContants.RoleNameClient, PermissionNames.ToolGroup_TurnOn, isGranted: false);
            await _permissionManager.SetForRoleAsync(CommonContants.RoleNameClient, PermissionNames.ToolGroup_TurnOff, isGranted: false);

            await _permissionManager.SetForRoleAsync(CommonContants.RoleNameClient, PermissionNames.DashboardGroup_Default, isGranted: false);
            #endregion

            //#region member's permissions
            //await _permissionManager.SetForRoleAsync(CommomContants.RoleNameMember, PermissionNames.ScriptGroup_Default, isGranted: true);
            //await _permissionManager.SetForRoleAsync(CommomContants.RoleNameMember, PermissionNames.ScriptGroup_RunScript, isGranted: false);
            //await _permissionManager.SetForRoleAsync(CommomContants.RoleNameMember, PermissionNames.ScriptGroup_Create, isGranted: false);
            //await _permissionManager.SetForRoleAsync(CommomContants.RoleNameMember, PermissionNames.ScriptGroup_Edit, isGranted: false);
            //await _permissionManager.SetForRoleAsync(CommomContants.RoleNameMember, PermissionNames.ScriptGroup_Delete, isGranted: false);

            //await _permissionManager.SetForRoleAsync(CommomContants.RoleNameMember, PermissionNames.ClientMachineGroup_Default, isGranted: true);
            //await _permissionManager.SetForRoleAsync(CommomContants.RoleNameMember, PermissionNames.ClientMachineGroup_Create, isGranted: false);
            //await _permissionManager.SetForRoleAsync(CommomContants.RoleNameMember, PermissionNames.ClientMachineGroup_Edit, isGranted: false);
            //await _permissionManager.SetForRoleAsync(CommomContants.RoleNameMember, PermissionNames.ClientMachineGroup_Default, isGranted: false);

            //await _permissionManager.SetForRoleAsync(CommomContants.RoleNameMember, PermissionNames.ToolGroup_Default, isGranted: false);
            //await _permissionManager.SetForRoleAsync(CommomContants.RoleNameMember, PermissionNames.ToolGroup_Create, isGranted: false);
            //await _permissionManager.SetForRoleAsync(CommomContants.RoleNameMember, PermissionNames.ToolGroup_Edit, isGranted: false);
            //await _permissionManager.SetForRoleAsync(CommomContants.RoleNameMember, PermissionNames.ToolGroup_Delete, isGranted: false);
            //#endregion
        }


    }
}
