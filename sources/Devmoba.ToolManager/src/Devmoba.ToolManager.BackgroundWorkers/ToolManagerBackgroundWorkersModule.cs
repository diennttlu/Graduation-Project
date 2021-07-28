using Devmoba.ToolManager.BackgroundWorkers.Workers;
using Devmoba.ToolManager.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Modularity;

namespace Devmoba.ToolManager.BackgroundWorkers
{
    [DependsOn(
        typeof(ToolManagerDomainModule),
        typeof(ToolManagerDomainSharedModule),
        typeof(ToolManagerEntityFrameworkCoreModule),
        typeof(AbpBackgroundWorkersModule)
        )]
    public class ToolManagerBackgroundWorkersModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.AddBackgroundWorker<ErrorWatchWorker>();
        }
    }
}
