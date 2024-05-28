namespace UranBot.Public.Database.Entities.Base;

public abstract class BaseDiscordEntity : BaseEntity
{
    public ulong DiscordId { get; set; }
}