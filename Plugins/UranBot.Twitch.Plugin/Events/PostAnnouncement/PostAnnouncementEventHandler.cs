using UranBot.Twitch.Plugin.Helper;

namespace UranBot.Twitch.Plugin.Events.PostAnnouncement;

public class PostAnnouncementEventHandler : IRequestHandler<PostAnnouncementEvent>
{
    private readonly IDiscordService _discordService;
    private readonly TwitchUranDbContext _dbContext;
    private readonly TwitchAPI _twitchApi;

    public PostAnnouncementEventHandler(IDiscordService discordService, TwitchAPI twitchApi, TwitchUranDbContext dbContext)
    {
        _discordService = discordService;
        _twitchApi = twitchApi;
        _dbContext = dbContext;
    }

    public async Task Handle(PostAnnouncementEvent request, CancellationToken cancellationToken)
    {
        TwitchAnnouncementSettings settings = _dbContext.Set<TwitchAnnouncementSettings>().Single(x => x.BroadcasterId == request.Broadcaster.Id);
        
        EmbedBuilder embedBuilder = await TwitchEmbedHelper.CreateAnnouncement(_twitchApi, request.Broadcaster, request.Stream);

        var messageId = await _discordService.SendMessage(settings.ChannelId, $"<@&{settings.Role.DiscordId}> {settings.Message}", embedBuilder.Build());

        _dbContext.Add(new TwitchAnnouncement()
        {
            BroadcasterId = settings.BroadcasterId, MessageId = messageId
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}