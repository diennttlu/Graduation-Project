namespace Devmoba.ToolManager.Permissions
{
    public static class ToolManagerPermissions
    {
        public const string GroupName = "ToolManager";

        public const string DashboardGroup = PermissionNames.DashboardGroup;
        public static class Dashboards
        {
            public const string Default = PermissionNames.DashboardGroup_Default;
        }

        public const string ClientMachineGroup = PermissionNames.ClientMachineGroup;
        public static class ClientMachines
        {
            public const string Default = PermissionNames.ClientMachineGroup_Default;
            public const string Create = PermissionNames.ClientMachineGroup_Create;
            public const string Edit = PermissionNames.ClientMachineGroup_Edit;
            public const string Delete = PermissionNames.ClientMachineGroup_Delete;
        }

        public const string ScriptGroup = PermissionNames.ScriptGroup;
        public static class Scripts
        {
            public const string Default = PermissionNames.ScriptGroup_Default;
            public const string Create = PermissionNames.ScriptGroup_Create;
            public const string Edit = PermissionNames.ScriptGroup_Edit;
            public const string Delete = PermissionNames.ScriptGroup_Delete;
            public const string RunScript = PermissionNames.ScriptGroup_RunScript;
        }

        public const string ToolGroup = PermissionNames.ToolGroup;
        public static class Tools
        {
            public const string Default = PermissionNames.ToolGroup_Default;
            //public const string Create = PermissionNames.ToolGroup_Create;
            public const string Edit = PermissionNames.ToolGroup_Edit;
            public const string Delete = PermissionNames.ToolGroup_Delete;
            public const string TurnOn = PermissionNames.ToolGroup_TurnOn;
            public const string TurnOff = PermissionNames.ToolGroup_TurnOff;
        }

        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";
    }
}