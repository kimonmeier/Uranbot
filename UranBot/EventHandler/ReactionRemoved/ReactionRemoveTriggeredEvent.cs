namespace UranBot.EventHandler.ReactionRemoved;

public class ReactionRemoveTriggeredEvent: IRequest
{
    public required ulong UserId { get; set; }
    
    public required DiscordReaction Reaction { get; set; }
}