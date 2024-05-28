namespace UranBot.Twitch.Plugin.Events.ClipRequestApproval;

public class ClipRequestApprovalEvent : IRequest
{
    public required TwitchClipSettings Settings { get; init; }
    
    public required Clip Clip { get; init; }
}