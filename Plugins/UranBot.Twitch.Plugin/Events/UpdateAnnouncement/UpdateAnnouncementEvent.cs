using TwitchLib.Api.Helix.Models.Channels.GetChannelInformation;
using Stream = TwitchLib.Api.Helix.Models.Streams.GetStreams.Stream;

namespace UranBot.Twitch.Plugin.Events.UpdateAnnouncement;

public class UpdateAnnouncementEvent : IRequest
{
    public required TwitchAnnouncement Announcement { get; set; }
    
    public required Stream Stream { get; set; }
}