using Stream = TwitchLib.Api.Helix.Models.Streams.GetStreams.Stream;

namespace UranBot.Twitch.Plugin.Events.PostAnnouncement;

public class PostAnnouncementEvent : IRequest
{
    public required TwitchBroadcaster Broadcaster { get; set; }
    
    public required Stream Stream { get; set; }
}