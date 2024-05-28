namespace UranBot.Public.Database;

public static class DatabaseExtensionsMethods
{
    public static DiscordUser GetUserByDiscordId(this DbContext dbContext, ulong id)
    {
        return dbContext.GetEntityByDiscordId<DiscordUser>(id);
    }
    
    public static long GetUserIdByDiscordId(this DbContext dbContext, ulong id)
    {
        return dbContext.GetUserByDiscordId(id).Id;
    }
    
    public static DiscordGuild GetGuildByDiscordId(this DbContext dbContext, ulong id)
    {
        return dbContext.GetEntityByDiscordId<DiscordGuild>(id);
    }
    
    public static long GetGuildIdByDiscordId(this DbContext dbContext, ulong id)
    {
        return dbContext.GetGuildByDiscordId(id).Id;
    }
    
    public static DiscordChannel GetChannelByDiscordId(this DbContext dbContext, ulong id)
    {
        return dbContext.GetEntityByDiscordId<DiscordChannel>(id);
    }
    
    public static long GetChannelIdByDiscordId(this DbContext dbContext, ulong id)
    {
        return dbContext.GetChannelByDiscordId(id).Id;
    }

    private static T GetEntityByDiscordId<T>(this DbContext dbContext, ulong id) where T : BaseDiscordEntity
    {
        return dbContext.Set<T>().Single(x => x.DiscordId == id);
    }
}