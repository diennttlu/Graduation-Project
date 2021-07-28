using Devmoba.ToolManager.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Devmoba.ToolManager.Repositories
{
    public class ToolRepository : EfCoreRepository<ToolManagerDbContext, Tool, long>, IToolRepository
    {
        public ToolRepository(IDbContextProvider<ToolManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public IQueryable<Tool> FullTextSearch(IQueryable<Tool> query, Expression<Func<Tool, string>> keySelector, string value)
        {
            return query.FullTextContains(keySelector, value);
        }
    }
}
