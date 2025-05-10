using UranBot.Database;
using UranBot.EventHandler.SyncGuild;
using UranBot.EventHandler.SyncUser;

namespace UranBot.EventHandler.SyncDatabase;

public class SyncDatabaseEventHandler : IRequestHandler<SyncDatabaseEvent>
{
    private readonly CoreUranDbContext _dbContext;
    private readonly DiscordSocketClient _client;
    private readonly ISender _sender;

    public SyncDatabaseEventHandler(CoreUranDbContext dbContext, DiscordSocketClient discordSocketClient, ISender sender)
    {
        _dbContext = dbContext;
        _client = discordSocketClient;
        _sender = sender;
    }

    public async Task Handle(SyncDatabaseEvent request, CancellationToken cancellationToken)
    {
        var guilds = _dbContext.Set<DiscordGuild>().ToList();

        List<Task> tasks = new();
        foreach (SocketGuild socketGuild in _client.Guilds)
        {
            tasks.Add(
                socketGuild.DownloadUsersAsync()
            );
        }
        
        await Task.WhenAll(tasks.ToArray());

        var users = _client.Guilds.SelectMany(x => x.Users);
        foreach (SocketGuildUser user in users)
        {
            await _sender.Send(new SyncUserEvent()
            {
                User = user
            }, cancellationToken);
        }

        foreach (SocketGuild socketGuild in _client.Guilds)
        {
            DiscordGuild? guild = guilds.SingleOrDefault(x => x.DiscordId == socketGuild.Id);
            if (guild is not null)
            {
                guilds.Remove(guild);
            }

            await _sender.Send(new SyncGuildEvent()
            {
                SocketGuild = socketGuild
            }, cancellationToken);
        }

        _dbContext.Set<DiscordGuild>().RemoveRange(guilds);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}