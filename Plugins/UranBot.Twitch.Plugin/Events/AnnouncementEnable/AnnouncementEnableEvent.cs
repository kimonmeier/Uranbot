namespace UranBot.Twitch.Plugin.Events.AnnouncementEnable;

public class AnnouncementEnableEvent : BaseCommandEvent
{
    public required string BroadcasterName { get; set; }
    
    public required SocketTextChannel Channel { get; set; }
    
    public required SocketRole Role { get; set; }
    
    public required string Message { get; set; }
}