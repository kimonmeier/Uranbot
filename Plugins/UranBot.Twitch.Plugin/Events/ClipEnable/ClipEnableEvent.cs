namespace UranBot.Twitch.Plugin.Events.ClipEnable;

public class ClipEnableEvent : BaseCommandEvent
{
    public required string BroadcasterName { get; init; }
    
    public required ISocketMessageChannel Channel { get; init; }
}