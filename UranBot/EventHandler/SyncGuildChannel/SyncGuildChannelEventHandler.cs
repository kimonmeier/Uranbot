using UranBot.Database;
using UranBot.EventHandler.SyncGuild;

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
            long guildId = _dbContext.GetGuildIdByDiscordId(request.Guild.Id);
            discordChannel = new DiscordChannel()
            {
                DiscordId = request.Channel.Id, GuildId = guildId, Name = request.Channel.Name
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