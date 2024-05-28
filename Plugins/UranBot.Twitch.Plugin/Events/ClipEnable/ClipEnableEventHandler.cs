namespace UranBot.Twitch.Plugin.Events.ClipEnable;

public class ClipEnableEventHandler : IRequestHandler<ClipEnableEvent>
{
    private readonly TwitchUranDbContext _dbContext;

    public ClipEnableEventHandler(TwitchUranDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(ClipEnableEvent request, CancellationToken cancellationToken)
    {
        TwitchBroadcaster? broadcaster = await _dbContext.Set<TwitchBroadcaster>().SingleOrDefaultAsync(x => x.BroadcasterName == request.BroadcasterName);
        if (broadcaster is null)
        {
            await request.SendFailureMessage($"The broadcaster {request.BroadcasterName} is unkown");
            return;
        }

        TwitchClipSettings? settings = await _dbContext.Set<TwitchClipSettings>().SingleOrDefaultAsync(x => x.BroadcasterId == broadcaster.Id);
        if (settings is not null)
        {
            await request.SendFailureMessage($"The clip sharing is already enabled for {request.BroadcasterName}");
            return;
        }

        _dbContext.Set<TwitchClipSettings>().Add(new TwitchClipSettings()
        {
            Broadcaster = _dbContext.Set<TwitchBroadcaster>().Single(x => x.BroadcasterName == request.BroadcasterName),
            DiscordChannelId = _dbContext.GetChannelIdByDiscordId(request.Channel.Id),
            LastSynched = DateTime.Now,
            ShareMode = TwitchClipShareMode.Automatic
        });
        await _dbContext.SaveChangesAsync(cancellationToken);

        await request.SendSuccessMessage($"The clip sharing was enabled for {request.BroadcasterName}");
    }
}