namespace UranBot.Twitch.Plugin.Events.AnnouncementDisable;

public class AnnouncementDisableEvent : BaseCommandEvent
{
    public required string BroadcasterName { get; set; }
    
}