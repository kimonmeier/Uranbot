﻿namespace UranBot.Twitch.Plugin.Events.ClipDisable;

public class ClipDisableEventHandler : IRequestHandler<ClipDisableEvent>
{
    private readonly TwitchUranDbContext _dbContext;

    public ClipDisableEventHandler(TwitchUranDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(ClipDisableEvent request, CancellationToken cancellationToken)
    {
        long guildId = _dbContext.GetEntityIdByDiscordId<DiscordGuild>(request.InteractionContext.Guild.Id)!.Value;
        TwitchBroadcaster? broadcaster = _dbContext.Set<TwitchBroadcaster>().SingleOrDefault(x => x.GuildId == guildId && x.BroadcasterName == request.BroadcasterName);
        if (broadcaster is null)
        {
            await request.SendFailureMessage($"The broadcaster {request.BroadcasterName} is unkown");
            return;
        }

        TwitchClipSettings? settings = _dbContext.Set<TwitchClipSettings>().SingleOrDefault(x => x.BroadcasterId == broadcaster.Id);
        if (settings is null)
        {
            await request.SendFailureMessage($"The clip sharing is already disabled for {request.BroadcasterName}");
            return;
        }

        _dbContext.Remove(settings);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await request.SendSuccessMessage($"The clip sharing for {request.BroadcasterName} is now disabled");
    }
}