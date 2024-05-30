namespace UranBot.Twitch.Plugin.Events.AnnouncementDisable;

public class AnnouncementDisableEventHandler : IRequestHandler<AnnouncementDisableEvent>
{
    private readonly TwitchUranDbContext _dbContext;

    public AnnouncementDisableEventHandler(TwitchUranDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(AnnouncementDisableEvent request, CancellationToken cancellationToken)
    {
        long guildId = _dbContext.GetEntityIdByDiscordId<DiscordGuild>(request.InteractionContext.Guild.Id)!.Value;
        TwitchBroadcaster? broadcaster = _dbContext.Set<TwitchBroadcaster>().SingleOrDefault(x => x.GuildId == guildId && x.BroadcasterName == request.BroadcasterName);
        if (broadcaster is null)
        {
            await request.SendFailureMessage($"The broadcaster {request.BroadcasterName} is unknown");
            return;
        }

        TwitchAnnouncementSettings? settings = _dbContext.Set<TwitchAnnouncementSettings>().SingleOrDefault(x => x.BroadcasterId == broadcaster.Id);
        if (settings is null)
        {
            await request.SendFailureMessage($"The announcement is already disabled for {request.BroadcasterName}");
            return;
        }

        _dbContext.Remove(settings);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await request.SendSuccessMessage($"The announcement for {request.BroadcasterName} is now disabled");
    }
}