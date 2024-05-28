namespace UranBot.Twitch.Plugin.Events.BroadcasterRemove;

public class BroadcasterRemoveEventHandler : IRequestHandler<BroadcasterRemoveEvent>
{
    private readonly TwitchUranDbContext _dbContext;

    public BroadcasterRemoveEventHandler(TwitchUranDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(BroadcasterRemoveEvent request, CancellationToken cancellationToken)
    {
        TwitchBroadcaster? twitchBroadcaster = await _dbContext.Set<TwitchBroadcaster>().FirstOrDefaultAsync(x => x.BroadcasterName == request.Name);

        if (twitchBroadcaster is null)
        {
            await request.SendFailureMessage($"The broadcaster {request.Name} was not found");
            return;
        }

        _dbContext.Remove(twitchBroadcaster);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await request.SendSuccessMessage($"The broadcaster {request.Name} was deleted!");
    }
}