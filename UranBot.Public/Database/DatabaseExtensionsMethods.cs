namespace UranBot.Public.Database;

public static class DatabaseExtensionsMethods
{
    public static T? GetEntityByDiscordId<T>(this DbContext dbContext, ulong id) where T : BaseDiscordEntity
    {
        T? entity = dbContext.Set<T>().SingleOrDefault(x => x.DiscordId == id);

        if (entity is null)
        {
            throw new Exception($"The Entity {typeof(T).Name} couldn't be found with the Id: {id}");
        }

        return entity;
    }

    public static long? GetEntityIdByDiscordId<T>(this DbContext dbContext, ulong id) where T : BaseDiscordEntity
    {
        var entityInfos = dbContext.Set<T>().Select(x => new { x.Id, x.DiscordId }).SingleOrDefault(x => x.DiscordId == id);
        
        if (entityInfos is null)
        {
            throw new Exception($"The Entity {typeof(T).Name} couldn't be found with the Id: {id}");
        }

        return entityInfos.Id;
    }
}