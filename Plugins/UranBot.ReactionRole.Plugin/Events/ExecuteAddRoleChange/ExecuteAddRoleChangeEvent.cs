using MediatR;
using UranBot.Public.Events;

namespace UranBot.ReactionRole.Plugin.Events.ExecuteAddRoleChange;

public class ExecuteAddRoleChangeEvent : BaseReactionContextEvent
{
    public ulong GuildId { get; init; }
    
    public ulong RoleId { get; init; }
    
}