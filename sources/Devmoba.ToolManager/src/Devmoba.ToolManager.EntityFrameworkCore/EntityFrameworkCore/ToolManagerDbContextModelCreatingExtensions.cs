using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Devmoba.ToolManager.EntityFrameworkCore
{
    public static class ToolManagerDbContextModelCreatingExtensions
    {
        public static void ConfigureToolManager(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<ClientMachine>(e =>
            {
                e.ToTable(ToolManagerConsts.DbTablePrefix + "ClientMachines", ToolManagerConsts.DbSchema);
                e.ConfigureByConvention();
                e.HasIndex(x => x.UserId).IsUnique();

                e.Property(x => x.IPLan).IsRequired().HasMaxLength(15);
                e.Property(x => x.IPPublic).IsRequired().HasMaxLength(15);
                e.Property(x => x.LastUpdate).IsRequired();
                e.Property(x => x.UserId).IsRequired();

                e.HasMany(x => x.Tools).WithOne(x => x.ClientMachine).HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Tool>(e =>
            {
                e.ToTable(ToolManagerConsts.DbTablePrefix + "Tools", ToolManagerConsts.DbSchema);
                e.ConfigureByConvention();
                e.HasIndex(x => new { x.AppId, x.ClientId }).IsUnique();

                e.Property(x => x.Name).IsRequired().HasMaxLength(256);
                e.Property(x => x.AppId).IsRequired();
                e.Property(x => x.ClientId).IsRequired();
                e.Property(x => x.ExeFilePath).IsRequired();
                e.Property(x => x.LastUpdate).IsRequired();
            });

            builder.Entity<Script>(e =>
            {
                e.ToTable(ToolManagerConsts.DbTablePrefix + "Scripts", ToolManagerConsts.DbSchema);
                e.ConfigureByConvention();
                e.Property(x => x.Name).IsRequired();
                e.Property(x => x.Content).IsRequired();
            });

            builder.Entity<Dependency>(e =>
            {
                e.ToTable(ToolManagerConsts.DbTablePrefix + "Dependencies", ToolManagerConsts.DbSchema);
                e.ConfigureByConvention();

                e.HasOne(x => x.Script).WithMany(x => x.Dependencies).HasForeignKey(x => x.ScriptId);
                e.HasOne(x => x.ScriptDependency).WithMany(x => x.ScriptDependencies).HasForeignKey(x => x.ScriptDependencyId).OnDelete(DeleteBehavior.NoAction);
            });

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(ToolManagerConsts.DbTablePrefix + "YourEntities", ToolManagerConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
        }
    }
}