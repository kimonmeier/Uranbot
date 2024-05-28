namespace UranBot.EventHandler.ReactionAdded;

public class ReactionTriggeredEvent : IRequest
{
    public required DiscordReaction Reaction { get; set; }
}