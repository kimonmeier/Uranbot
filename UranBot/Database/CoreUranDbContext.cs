using System.Reflection;
using UranBot.Public.Database.Entities.Base;

namespace UranBot.Database;

public sealed class CoreUranDbContext : BaseUranDbContext
{
    protected override string Name => "Core";

    protected override Assembly Assembly => typeof(IDiscordService).Assembly;

    public CoreUranDbContext(DbContextOptions<CoreUranDbContext> options) : base(options)
    {
    }
}