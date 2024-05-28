namespace UranBot.Twitch.Plugin.Events.ClipUpdate;

public class ClipUpdateEvent : IRequest
{
    public required TwitchClip TwitchClip { get; init; }
    
    public required Clip Clip { get; init; }
}