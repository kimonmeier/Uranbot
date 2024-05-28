namespace UranBot.Public.Database.Entities;

public class DiscordGuildMember : BaseEntity
{
    public long GuildId { get; set; }
    
    public virtual DiscordGuild Guild { get; set; }
    
    public long UserId { get; set; }
    
    public virtual DiscordUser User { get; set; }
}