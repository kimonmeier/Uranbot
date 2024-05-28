namespace UranBot.Public.Database.Entities;

public class DiscordChannel : BaseDiscordEntity
{
    public string Name { get; set; }
    
    public long GuildId { get; set; }
    
    public virtual DiscordGuild Guild { get; set; }
}