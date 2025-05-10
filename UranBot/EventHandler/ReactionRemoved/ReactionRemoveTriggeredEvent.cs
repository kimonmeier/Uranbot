namespace UranBot.EventHandler.ReactionRemoved;

public class ReactionRemoveTriggeredEvent: IRequest
{
    public required DiscordReaction Reaction { get; set; }
}