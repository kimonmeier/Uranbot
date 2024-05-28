using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace UranBot.Public.Database;

public abstract class BaseUranDbContext : DbContext
{
    protected abstract string Name { get; }
    
    protected abstract Assembly Assembly { get; }
    
    protected BaseUranDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName?.StartsWith("UranBot") ?? false))
        {
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }

        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.Assembly == Assembly)
            {
                continue;
            }
            
            modelBuilder.Entity(entityType.ClrType).Metadata.SetIsTableExcludedFromMigrations(true);
        }

        modelBuilder.Ignore<BaseEvent>();
        
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=.\\database.db;", op =>
        {
            op.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            op.MigrationsHistoryTable($"__EFMigrationHistoryTable{Name}");
        });

        optionsBuilder.UseLazyLoadingProxies();
    }
}