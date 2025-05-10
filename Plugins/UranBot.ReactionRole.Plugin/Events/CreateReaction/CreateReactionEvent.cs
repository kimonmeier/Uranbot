using Discord;
using UranBot.Public.Events;

namespace UranBot.ReactionRole.Plugin.Events.CreateReaction;

public class CreateReactionEvent : BaseCommandEvent
{
    public ITextChannel Channel { get; init; } = null!;

    public IRole Role { get; init; } = null!;
    
    public ulong MessageId { get; init; }
    
    public Emoji Emote { get; init; } = null!;
}