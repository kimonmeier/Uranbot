namespace UranBot.Twitch.Plugin.Events.ClipRequestApproved;

public class ClipRequestApprovedEvent : IRequest<bool>
{
    public required long BroadcastId { get; init; }
    
    public required Clip Clip { get; init; }
}