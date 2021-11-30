using Devmoba.ToolManager.EntityFrameworkCore;
using EFCore.BulkExtensions;
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
        private static readonly List<string> BulkUpdateIncludeProperties = new List<string>()
        {
            nameof(Tool.ProcessState),
            nameof(Tool.LastUpdate)
        }; 

        public ToolRepository(IDbContextProvider<ToolManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task BulkUpdateAsync(List<Tool> tools, int batchSize)
        {
            await DbContext.BulkUpdateAsync(tools, new BulkConfig() { BatchSize = batchSize, PropertiesToInclude = BulkUpdateIncludeProperties });
        }

        public IQueryable<Tool> FullTextSearch(IQueryable<Tool> query, Expression<Func<Tool, string>> keySelector, string value)
        {
            return query.FullTextContains(keySelector, value);
        }
    }
}
