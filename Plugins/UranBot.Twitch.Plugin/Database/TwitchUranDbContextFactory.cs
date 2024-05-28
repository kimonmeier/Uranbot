using Microsoft.EntityFrameworkCore.Design;

namespace UranBot.Twitch.Plugin.Database;

public class TwitchUranDbContextFactory : IDesignTimeDbContextFactory<TwitchUranDbContext>
{
    public TwitchUranDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TwitchUranDbContext>();
        optionsBuilder.UseSqlite("Data Source=database.db");

        return new TwitchUranDbContext(optionsBuilder.Options);
    }
}