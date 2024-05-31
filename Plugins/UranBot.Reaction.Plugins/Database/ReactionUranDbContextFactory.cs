using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UranBot.Reaction.Plugins.Database;

public class ReactionUranDbContextFactory : IDesignTimeDbContextFactory<ReactionUranDbContext>
{
    public ReactionUranDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ReactionUranDbContext>();
        optionsBuilder.UseSqlite("Data Source=database.db");

        return new ReactionUranDbContext(optionsBuilder.Options);
    }
}