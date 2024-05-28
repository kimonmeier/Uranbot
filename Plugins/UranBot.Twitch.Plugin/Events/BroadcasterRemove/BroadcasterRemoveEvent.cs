namespace UranBot.Twitch.Plugin.Events.BroadcasterRemove;

public class BroadcasterRemoveEvent : BaseCommandEvent
{
    public required string Name { get; init; }
    
}