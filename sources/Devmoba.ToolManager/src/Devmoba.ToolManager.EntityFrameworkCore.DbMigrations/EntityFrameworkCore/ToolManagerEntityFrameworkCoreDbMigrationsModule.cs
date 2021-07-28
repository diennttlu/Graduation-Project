using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Devmoba.ToolManager.EntityFrameworkCore
{
    [DependsOn(
        typeof(ToolManagerEntityFrameworkCoreModule)
        )]
    public class ToolManagerEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ToolManagerMigrationsDbContext>();
        }
    }
}
