using UranBot.Public.Database.Entities;
using UranBot.Twitch.Plugin.Database.Enums;

namespace UranBot.Twitch.Plugin.Database.Entities;

public class TwitchClipSettings : BaseEntity
{
    public long DiscordChannelId { get; set; }

    public virtual DiscordChannel DiscordChannel { get; set; }
    
    public long? ApprovalChannelId { get; set; }

    public virtual DiscordChannel? ApprovalChannel { get; set; }

    public long BroadcasterId { get; set; }

    public virtual TwitchBroadcaster Broadcaster { get; set; }
    
    public TwitchClipShareMode ShareMode { get; set; }

    public DateTime LastSynched { get; set; }
}