using UranBot.Twitch.Plugin.Helper;

namespace UranBot.Twitch.Plugin.Events.UpdateAnnouncement;

public class UpdateAnnouncementEventHandler : IRequestHandler<UpdateAnnouncementEvent>
{
    private readonly IDiscordService _discordService;
    private readonly TwitchAPI _twitchApi;
    private readonly TwitchUranDbContext _dbContext;

    public UpdateAnnouncementEventHandler(IDiscordService discordService, TwitchAPI twitchApi, TwitchUranDbContext dbContext)
    {
        _discordService = discordService;
        _twitchApi = twitchApi;
        _dbContext = dbContext;
    }

    public async Task Handle(UpdateAnnouncementEvent request, CancellationToken cancellationToken)
    {
        EmbedBuilder embedBuilder = await TwitchEmbedHelper.CreateAnnouncement(_twitchApi, request.Announcement.Broadcaster, request.Stream);
        IMessage message = await _discordService.GetMessage(request.Announcement.Message);
        EmbedBuilder? postedBuilder = message.Embeds.FirstOrDefault()?.ToEmbedBuilder();
        if (postedBuilder?.Equals(embedBuilder.Build().ToEmbedBuilder()) ?? false)
        {
            return;
        }

        await _discordService.UpdateMessage(request.Announcement.Message, embedBuilder.Build());
    }
}