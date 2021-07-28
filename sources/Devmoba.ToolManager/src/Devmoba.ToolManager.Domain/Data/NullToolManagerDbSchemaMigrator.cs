using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Devmoba.ToolManager.Data
{
    /* This is used if database provider does't define
     * IToolManagerDbSchemaMigrator implementation.
     */
    public class NullToolManagerDbSchemaMigrator : IToolManagerDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}