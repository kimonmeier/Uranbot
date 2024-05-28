using System.Reflection;

namespace UranBot.Twitch.Plugin.Database;

public sealed class TwitchUranDbContext : BaseUranDbContext
{
    protected override string Name => "Twitch";

    protected override Assembly Assembly => typeof(TwitchUranDbContext).Assembly;

    public TwitchUranDbContext(DbContextOptions<TwitchUranDbContext> options) : base(options)
    {
    }
}