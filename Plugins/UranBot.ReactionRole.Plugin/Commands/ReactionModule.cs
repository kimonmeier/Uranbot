using Discord;
using Discord.Interactions;
using SQLitePCL;
using UranBot.Public.Database.Entities;
using UranBot.Public.Services;
using UranBot.ReactionRole.Plugin.Events.CreateReaction;

namespace UranBot.ReactionRole.Plugin.Commands;

[Group("reaction", "Reaction Role Commands")]
[RequireUserPermission(GuildPermission.ManageRoles | GuildPermission.Administrator)]
public class ReactionModule : InteractionModuleBase
{
    private readonly IUranInteractionService _interactionService;

    public ReactionModule(IUranInteractionService interactionService)
    {
        _interactionService = interactionService;
    }

    [SlashCommand("create", "Create a message with reaction for a specific role")]
    public Task CreateRole(ITextChannel channel, string discordMessageId, IRole role, string emoteName)
    {
        return _interactionService.ExecuteInteraction(new CreateReactionEvent()
        {
            Channel = channel,
            Role = role,
            Emote = Emoji.Parse(emoteName),
            MessageId = ulong.Parse(discordMessageId),
            InteractionContext = Context
        });
    }
}