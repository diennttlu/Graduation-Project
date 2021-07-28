using Devmoba.ToolManager.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Devmoba.ToolManager.Repositories
{
    public class DependencyRepository : EfCoreRepository<ToolManagerDbContext, Dependency, long>, IDependencyRepository
    {
        public DependencyRepository(IDbContextProvider<ToolManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
