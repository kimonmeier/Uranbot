namespace UranBot.Twitch.Plugin.Events.DeleteAnnouncement;

public class DeleteAnnouncementEventHandler : IRequestHandler<DeleteAnnouncementEvent>
{
    private readonly IDiscordService _discordService;
    private readonly TwitchUranDbContext _dbContext;

    public DeleteAnnouncementEventHandler(IDiscordService discordService, TwitchUranDbContext dbContext)
    {
        _discordService = discordService;
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteAnnouncementEvent request, CancellationToken cancellationToken)
    {
        await _discordService.DeleteMessage(request.Announcement.Message);
        _dbContext.Remove(request.Announcement);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}