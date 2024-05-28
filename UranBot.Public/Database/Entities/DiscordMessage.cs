namespace UranBot.Public.Database.Entities;

public class DiscordMessage : BaseDiscordEntity
{
    public string? Content { get; set; }
    
    public long ChannelId { get; set; }
    
    public virtual DiscordChannel Channel { get; set; }
    
    public long? UserId { get; set; }
    
    public virtual DiscordUser? User { get; set; }
    
    public bool IsDeleted { get; set; }
}