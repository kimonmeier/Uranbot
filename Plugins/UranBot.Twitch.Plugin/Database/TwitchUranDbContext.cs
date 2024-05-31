using System.Reflection;

namespace UranBot.Twitch.Plugin.Database;

public sealed class TwitchUranDbContext(DbContextOptions<TwitchUranDbContext> options) : BaseUranDbContext(options)
{
    protected override string Name => "Twitch";

    protected override Assembly Assembly => typeof(TwitchUranDbContext).Assembly;
}