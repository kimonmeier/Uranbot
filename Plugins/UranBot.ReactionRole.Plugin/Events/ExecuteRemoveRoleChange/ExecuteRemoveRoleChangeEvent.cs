using UranBot.Public.Events;

namespace UranBot.ReactionRole.Plugin.Events.ExecuteRemoveRoleChange;

public class ExecuteRemoveRoleChangeEvent : BaseReactionContextEvent
{
    public ulong GuildId { get; init; }
    
    public ulong RoleId { get; init; }
}