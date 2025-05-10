using MediatR;
using UranBot.Public.Services;

namespace UranBot.ReactionRole.Plugin.Events.ExecuteAddRoleChange;

public class ExecuteAddRoleChangeEventHandler : IRequestHandler<ExecuteAddRoleChangeEvent, bool>
{
    private readonly IDiscordService _discordService;

    public ExecuteAddRoleChangeEventHandler(IDiscordService discordService)
    {
        _discordService = discordService;
    }

    public async Task<bool> Handle(ExecuteAddRoleChangeEvent request, CancellationToken cancellationToken)
    {
        await _discordService.AddRoleToUser(request.GuildId, request.UserId, request.RoleId);

        return false;
    }
}