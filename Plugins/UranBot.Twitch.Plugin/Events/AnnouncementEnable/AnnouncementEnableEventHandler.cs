namespace UranBot.Twitch.Plugin.Events.AnnouncementEnable;

public sealed class AnnouncementEnableEventHandler : IRequestHandler<AnnouncementEnableEvent>
{
    private readonly TwitchUranDbContext _dbContext;

    public AnnouncementEnableEventHandler(TwitchUranDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(AnnouncementEnableEvent request, CancellationToken cancellationToken)
    {
        long guildId = _dbContext.GetEntityIdByDiscordId<DiscordGuild>(request.InteractionContext.Guild.Id)!.Value;
        TwitchBroadcaster? broadcaster = _dbContext.Set<TwitchBroadcaster>().SingleOrDefault(x => x.GuildId == guildId && x.BroadcasterName == request.BroadcasterName);
        if (broadcaster is null)
        {
            await request.SendFailureMessage($"The broadcaster {request.BroadcasterName} is unknown");

            return;
        }

        TwitchAnnouncementSettings? settings = _dbContext.Set<TwitchAnnouncementSettings>().SingleOrDefault(x => x.BroadcasterId == broadcaster.Id);
        if (settings is not null)
        {
            await request.SendFailureMessage($"The announcement is already enabled for {request.BroadcasterName}");
            return;
        }

        _dbContext.Add(new TwitchAnnouncementSettings()
        {
            Broadcaster = broadcaster,
            Message = request.Message,
            ChannelId = _dbContext.GetEntityIdByDiscordId<DiscordChannel>(request.Channel.Id)!.Value,
            RoleId = _dbContext.GetEntityIdByDiscordId<DiscordRole>(request.Role.Id)!.Value,
        });
        await _dbContext.SaveChangesAsync(cancellationToken);
        await request.SendSuccessMessage($"The announcement was enabled for {request.BroadcasterName}");
    }
}