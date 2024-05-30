using UranBot.Database;

namespace UranBot.EventHandler.SyncGuildRole;

public class SyncGuildRoleEventHandler : IRequestHandler<SyncGuildRoleEvent>
{
    private readonly CoreUranDbContext _dbContext;

    public SyncGuildRoleEventHandler(CoreUranDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(SyncGuildRoleEvent request, CancellationToken cancellationToken)
    {
        DiscordRole? role = _dbContext.Set<DiscordRole>().SingleOrDefault(x => x.DiscordId == request.Role.Id);
        
        if (role is null)
        {
            long? guildId = _dbContext.GetEntityIdByDiscordId<DiscordGuild>(request.Guild.Id);

            if (guildId is null)
            {
                throw new Exception($"The Guild for Role {request.Role.Id} couldn't be found");
            }
            
            role = new DiscordRole()
            {
                DiscordId = request.Role.Id, GuildId = guildId.Value
            };
            
            _dbContext.Set<DiscordRole>().Add(role);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}