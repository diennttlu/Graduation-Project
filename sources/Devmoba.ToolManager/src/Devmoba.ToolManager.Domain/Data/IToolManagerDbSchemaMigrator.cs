using System.Threading.Tasks;

namespace Devmoba.ToolManager.Data
{
    public interface IToolManagerDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
