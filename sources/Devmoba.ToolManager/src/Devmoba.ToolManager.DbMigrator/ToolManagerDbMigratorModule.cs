using Devmoba.ToolManager.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Devmoba.ToolManager.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(ToolManagerEntityFrameworkCoreDbMigrationsModule),
        typeof(ToolManagerApplicationContractsModule)
        )]
    public class ToolManagerDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
