namespace UranBot.Twitch.Plugin.Events.ClipSharedModeChange;

public class ClipSharedModeChangeEventHandler : IRequestHandler<ClipSharedModeChangeEvent>
{
    private readonly TwitchUranDbContext _dbContext;

    public ClipSharedModeChangeEventHandler(TwitchUranDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(ClipSharedModeChangeEvent request, CancellationToken cancellationToken)
    {
        TwitchBroadcaster? broadcaster = await _dbContext.Set<TwitchBroadcaster>().SingleOrDefaultAsync(x => x.BroadcasterName == request.BroadcasterName);
        if (broadcaster is null)
        {
            await request.SendFailureMessage($"The broadcaster {request.BroadcasterName} is unkown");

            return;
        }

        TwitchClipSettings? settings = await _dbContext.Set<TwitchClipSettings>().SingleOrDefaultAsync(x => x.BroadcasterId == broadcaster.Id);
        if (settings is null)
        {
            await request.SendFailureMessage($"The clip sharing is not enabled for {request.BroadcasterName}");

            return;
        }

        if (request.ShareMode == TwitchClipShareMode.Approval && request.ApprovalChannel is null)
        {
            await request.SendFailureMessage($"An approval channel for the clips must be submitted");

            return;
        }

        if (request.ApprovalChannel is null)
        {
            settings.ApprovalChannelId = null;
        }
        else
        {
            settings.ApprovalChannelId = _dbContext.GetChannelIdByDiscordId(request.ApprovalChannel.Id);
        }

        settings.ShareMode = request.ShareMode;
        await _dbContext.SaveChangesAsync(cancellationToken);

        await request.SendSuccessMessage($"The settings for {request.BroadcasterName} were updated");
    }
}