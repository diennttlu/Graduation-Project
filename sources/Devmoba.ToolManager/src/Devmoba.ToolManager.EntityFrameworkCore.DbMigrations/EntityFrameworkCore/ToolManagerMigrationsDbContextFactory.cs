using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Devmoba.ToolManager.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class ToolManagerMigrationsDbContextFactory : IDesignTimeDbContextFactory<ToolManagerMigrationsDbContext>
    {
        public ToolManagerMigrationsDbContext CreateDbContext(string[] args)
        {
            ToolManagerEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ToolManagerMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new ToolManagerMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Devmoba.ToolManager.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
