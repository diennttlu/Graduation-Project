using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Devmoba.ToolManager.HttpApi.Client.ConsoleTestApp
{
    [DependsOn(
        typeof(ToolManagerHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class ToolManagerConsoleApiClientModule : AbpModule
    {
        
    }
}
