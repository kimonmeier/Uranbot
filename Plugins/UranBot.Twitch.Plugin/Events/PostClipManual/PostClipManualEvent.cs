namespace UranBot.Twitch.Plugin.Events.PostClipManual;

public class PostClipManualEvent : BaseCommandEvent
{
    public required string ClipUrl { get; set; }
}