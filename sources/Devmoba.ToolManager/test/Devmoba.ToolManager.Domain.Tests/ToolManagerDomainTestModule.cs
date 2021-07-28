using Devmoba.ToolManager.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Devmoba.ToolManager
{
    [DependsOn(
        typeof(ToolManagerEntityFrameworkCoreTestModule)
        )]
    public class ToolManagerDomainTestModule : AbpModule
    {

    }
}