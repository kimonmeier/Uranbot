using UranBot.Database;

namespace UranBot.EventHandler.SyncGuildMember;

public class SyncGuildMemberEventHandler : IRequestHandler<SyncGuildMemberEvent, bool>
{
    private readonly CoreUranDbContext _dbContext;

    public SyncGuildMemberEventHandler(CoreUranDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(SyncGuildMemberEvent request, CancellationToken cancellationToken)
    {
        long? userId = _dbContext.GetEntityIdByDiscordId<DiscordUser>(request.User.Id);
        long? guildId = _dbContext.GetEntityIdByDiscordId<DiscordGuild>(request.Guild.Id);

        if (userId is null || guildId is null)
        {
            throw new Exception("The User or Guild couldn't be found");
        }

        bool retVal = false;
        if (!_dbContext.Set<DiscordGuildMember>().Any(x => x.UserId == userId && x.GuildId == guildId))
        {
            _dbContext.Add(new DiscordGuildMember()
            {
                GuildId = guildId.Value, UserId = userId.Value
            });
        }
        else
        {
            retVal = true;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return retVal;
    }
}