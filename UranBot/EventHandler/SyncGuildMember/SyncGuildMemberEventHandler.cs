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
        long userId = _dbContext.GetUserIdByDiscordId(request.User.Id);
        long guildId = _dbContext.GetGuildIdByDiscordId(request.Guild.Id);

        bool retVal = false;
        if (!_dbContext.Set<DiscordGuildMember>().Any(x => x.UserId == userId && x.GuildId == guildId))
        {
            _dbContext.Add(new DiscordGuildMember()
            {
                GuildId = guildId, UserId = userId
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