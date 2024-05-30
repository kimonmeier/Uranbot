using UranBot.Database;
using UranBot.EventHandler.SyncGuildChannel;
using UranBot.EventHandler.SyncGuildMember;
using UranBot.EventHandler.SyncGuildRole;

namespace UranBot.EventHandler.SyncGuild;

public class SyncGuildEventHandler : IRequestHandler<SyncGuildEvent, DiscordGuild>
{
    private readonly CoreUranDbContext _dbContext;
    private readonly ISender _sender;

    public SyncGuildEventHandler(CoreUranDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task<DiscordGuild> Handle(SyncGuildEvent request, CancellationToken cancellationToken)
    {
        DiscordGuild? discordGuild = _dbContext.Set<DiscordGuild>().FirstOrDefault(x => x.DiscordId == request.SocketGuild.Id);
        if (discordGuild is null)
        {
            discordGuild = new DiscordGuild()
            {
                DiscordId = request.SocketGuild.Id
            };

            _dbContext.Add(discordGuild);
        }
        else
        {
            // TODO: Update schreiben
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        foreach (SocketTextChannel textChannel in request.SocketGuild.TextChannels)
        {
            await _sender.Send(new SyncGuildChannelEvent()
            {
                Guild = request.SocketGuild, Channel = textChannel
            }, cancellationToken);
        }
        
        foreach (SocketGuildUser user in request.SocketGuild.Users)
        {
            await _sender.Send(new SyncGuildMemberEvent()
            {
                Guild = request.SocketGuild, User = user
            }, cancellationToken);
        }
        
        foreach (SocketRole role in request.SocketGuild.Roles)
        {
            await _sender.Send(new SyncGuildRoleEvent()
            {
                Guild = request.SocketGuild, Role = role
            }, cancellationToken);
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return discordGuild;
    }
}