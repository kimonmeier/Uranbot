namespace UranBot.Twitch.Plugin.Database.Entities;

public class TwitchBroadcaster : BaseEntity
{
    public string BroadcasterName { get; set; }
    
    public string TwitchId { get; set; }
    
    public long GuildId { get; set; }
    
    public virtual DiscordGuild Guild { get; set; }
}