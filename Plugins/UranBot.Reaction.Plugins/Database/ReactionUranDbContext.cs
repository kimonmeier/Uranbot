using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UranBot.Public.Database;

namespace UranBot.Reaction.Plugins.Database;

public class ReactionUranDbContext(DbContextOptions options) : BaseUranDbContext(options)
{
    protected override string Name => "Reaction";
    protected override Assembly Assembly => typeof(ReactionUranDbContext).Assembly;
}