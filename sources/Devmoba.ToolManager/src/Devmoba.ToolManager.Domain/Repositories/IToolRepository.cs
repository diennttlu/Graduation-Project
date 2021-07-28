using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Devmoba.ToolManager.Repositories
{
    public interface IToolRepository : IRepository<Tool, long>
    {
        IQueryable<Tool> FullTextSearch(IQueryable<Tool> query, Expression<Func<Tool, string>> keySelector, string value);
    }
}
