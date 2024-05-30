namespace UranBot.Twitch.Plugin.Events.DeleteAnnouncement;

public class DeleteAnnouncementEvent : IRequest
{
    public required TwitchAnnouncement Announcement { get; set; }
    
}