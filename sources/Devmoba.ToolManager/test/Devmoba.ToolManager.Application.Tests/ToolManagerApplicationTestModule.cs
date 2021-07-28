using Volo.Abp.Modularity;

namespace Devmoba.ToolManager
{
    [DependsOn(
        typeof(ToolManagerApplicationModule),
        typeof(ToolManagerDomainTestModule)
        )]
    public class ToolManagerApplicationTestModule : AbpModule
    {

    }
}