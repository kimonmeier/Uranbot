namespace UranBot.Public.Database.Entities;

public class DiscordGuild : BaseDiscordEntity
{
    public long? OwnerId { get; set; }
    
    public virtual DiscordUser? Owner { get; set; }
}