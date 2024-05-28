namespace UranBot.Twitch.Plugin.Database.Entities;

public class TwitchBroadcaster : BaseEntity
{
    public string BroadcasterName { get; set; }
    
    public required string TwitchId { get; set; }
}