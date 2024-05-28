namespace UranBot.Twitch.Plugin.Events.ClipSharedModeChange;

public class ClipSharedModeChangeEvent : BaseCommandEvent
{
    public required TwitchClipShareMode ShareMode { get; init; }
    
    public required string BroadcasterName { get; init; }
    
    public ISocketMessageChannel? ApprovalChannel { get; init; } 
}