namespace UranBot.EventHandler.ReactionAdded;

public class ReactionAddTriggeredEvent : IRequest
{
    public required ulong UserId { get; set; }
    
    public required DiscordReaction Reaction { get; set; }
}