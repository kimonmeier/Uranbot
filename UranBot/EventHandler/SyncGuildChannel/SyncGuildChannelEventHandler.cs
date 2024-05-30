using UranBot.Database;

namespace UranBot.EventHandler.SyncGuildChannel;

public class SyncGuildChannelEventHandler : IRequestHandler<SyncGuildChannelEvent>
{
    private readonly CoreUranDbContext _dbContext;

    public SyncGuildChannelEventHandler(CoreUranDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(SyncGuildChannelEvent request, CancellationToken cancellationToken)
    {
        DiscordChannel? discordChannel = await _dbContext.Set<DiscordChannel>().SingleOrDefaultAsync(x => x.DiscordId == request.Channel.Id, cancellationToken: cancellationToken);
        
        if (discordChannel is null)
        {
            long? guildId = _dbContext.GetEntityIdByDiscordId<DiscordGuild>(request.Guild.Id);

            if (guildId is null)
            {
                throw new Exception($"The Guild for Channel {request.Channel.Id} couldn't be found");
            }
            
            discordChannel = new DiscordChannel()
            {
                DiscordId = request.Channel.Id, GuildId = guildId.Value, Name = request.Channel.Name
            };
            
            _dbContext.Set<DiscordChannel>().Add(discordChannel);
        }
        else
        {
            discordChannel.Name = request.Channel.Name;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}