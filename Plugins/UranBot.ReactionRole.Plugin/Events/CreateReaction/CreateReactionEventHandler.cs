using MediatR;
using Microsoft.EntityFrameworkCore;
using UranBot.Public.Services;
using UranBot.ReactionRole.Plugin.Events.ExecuteAddRoleChange;
using UranBot.ReactionRole.Plugin.Events.ExecuteRemoveRoleChange;

namespace UranBot.ReactionRole.Plugin.Events.CreateReaction;

public class CreateReactionEventHandler : IRequestHandler<CreateReactionEvent>
{
    private readonly IReactionService _reactionService;
    private readonly IDiscordService _discordService;

    public CreateReactionEventHandler(IReactionService reactionService, IDiscordService discordService)
    {
        _reactionService = reactionService;
        _discordService = discordService;
    }

    public async Task Handle(CreateReactionEvent request, CancellationToken cancellationToken)
    {
        await _reactionService.AddReaction(request.Channel.Id, request.MessageId, request.Emote,
            new ExecuteAddRoleChangeEvent()
            {
                UserId = ulong.MinValue,
                GuildId = request.InteractionContext.Guild.Id,
                RoleId = request.Role.Id
            },
            new ExecuteRemoveRoleChangeEvent()
            {
                UserId = ulong.MinValue,
                GuildId = request.InteractionContext.Guild.Id,
                RoleId = request.Role.Id
            });
    }
}