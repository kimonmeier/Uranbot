namespace UranBot.Twitch.Plugin.Events.PostClip;

public class PostClipEvent : IRequest
{
    public required TwitchClipSettings Settings { get; init; }
    
    public required Clip Clip { get; init; }
}