namespace UranBot.Twitch.Plugin.Events.ClipDisable;

public class ClipDisableEvent : BaseCommandEvent
{
    public required string BroadcasterName { get; init; }
    
}