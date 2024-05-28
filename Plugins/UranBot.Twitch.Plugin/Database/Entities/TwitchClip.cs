using UranBot.Public.Database.Entities;

namespace UranBot.Twitch.Plugin.Database.Entities;

public class TwitchClip : BaseEntity
{
    public long DiscordMessageId { get; set; }
    
    public virtual DiscordMessage DiscordMessage { get; set; }
    
    public string ClipId { get; set; }
    
    public long BroadcasterId { get; set; }
    
    public virtual TwitchBroadcaster Broadcaster { get; set; }
    
    public DateTime PostedAt { get; set; }
}