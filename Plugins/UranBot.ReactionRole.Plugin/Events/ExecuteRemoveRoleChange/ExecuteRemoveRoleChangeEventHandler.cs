using MediatR;
using UranBot.Public.Services;

namespace UranBot.ReactionRole.Plugin.Events.ExecuteRemoveRoleChange;

public class ExecuteRemoveRoleChangeEventHandler : IRequestHandler<ExecuteRemoveRoleChangeEvent, bool>
{
    private readonly IDiscordService _discordService;

    public ExecuteRemoveRoleChangeEventHandler(IDiscordService discordService)
    {
        _discordService = discordService;
    }

    public async Task<bool> Handle(ExecuteRemoveRoleChangeEvent request, CancellationToken cancellationToken)
    {
        await _discordService.RemoveRoleFromUser(request.GuildId, request.UserId, request.RoleId);

        return false;
    }
}