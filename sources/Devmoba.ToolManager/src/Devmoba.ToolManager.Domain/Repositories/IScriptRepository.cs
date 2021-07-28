using System;
using System.Linq;
using System.Linq.Expressions;
using Volo.Abp.Domain.Repositories;

namespace Devmoba.ToolManager.Repositories
{
    public interface IScriptRepository : IRepository<Script, long>
    {
        IQueryable<Script> FullTextSearch(IQueryable<Script> query, Expression<Func<Script, string>> keySelector, string value);
    }
}
