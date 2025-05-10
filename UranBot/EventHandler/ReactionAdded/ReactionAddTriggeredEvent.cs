namespace UranBot.EventHandler.ReactionAdded;

public class ReactionAddTriggeredEvent : IRequest
{
    public required DiscordReaction Reaction { get; set; }
}