using Microsoft.EntityFrameworkCore.Design;

namespace UranBot.Database;

public class CoreUranDbContextFactory : IDesignTimeDbContextFactory<CoreUranDbContext>
{
    public CoreUranDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CoreUranDbContext>();
        optionsBuilder.UseSqlite("Data Source=database.db");

        return new CoreUranDbContext(optionsBuilder.Options);
    }
}