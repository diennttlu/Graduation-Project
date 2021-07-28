using Volo.Abp.Settings;

namespace Devmoba.ToolManager.Settings
{
    public class ToolManagerSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(ToolManagerSettings.MySetting1));
        }
    }
}
