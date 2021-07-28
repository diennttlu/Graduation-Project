using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Devmoba.ToolManager.Data;
using Volo.Abp.DependencyInjection;

namespace Devmoba.ToolManager.EntityFrameworkCore
{
    public class EntityFrameworkCoreToolManagerDbSchemaMigrator
        : IToolManagerDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreToolManagerDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the ToolManagerMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<ToolManagerMigrationsDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}