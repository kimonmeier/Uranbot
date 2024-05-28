namespace UranBot.Twitch.Plugin.Events.DeleteClip;

public class DeleteClipEvent : BaseCommandEvent
{
    public required string ClipUrl { get; set; }
    
}