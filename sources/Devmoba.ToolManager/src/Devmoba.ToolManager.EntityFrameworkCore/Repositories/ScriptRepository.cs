using Devmoba.ToolManager.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Devmoba.ToolManager.Repositories
{
    public class ScriptRepository : EfCoreRepository<ToolManagerDbContext, Script, long>, IScriptRepository
    {
        public ScriptRepository(IDbContextProvider<ToolManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public IQueryable<Script> FullTextSearch(IQueryable<Script> query, Expression<Func<Script, string>> keySelector, string value)
        {
            return query.FullTextContains(keySelector, value);
        }

     
    }
}
