using UranBot.Database;

namespace UranBot.EventHandler.SyncUser;

public class SyncUserEventHandler: IRequestHandler<SyncUserEvent>
{
    private readonly CoreUranDbContext _dbContext;

    public SyncUserEventHandler(CoreUranDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(SyncUserEvent request, CancellationToken cancellationToken)
    {
        if (!_dbContext.Set<DiscordUser>().Any(x => x.DiscordId == request.User.Id))
        {
            _dbContext.Set<DiscordUser>().Add(new DiscordUser()
            {
                DiscordId = request.User.Id
            });
        }
        else
        {
            // TODO: Update schreiben
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}