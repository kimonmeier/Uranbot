namespace UranBot.Twitch.Plugin.Events.DeleteAnnouncement;

public class DeleteAnnouncementEventHandler : IRequestHandler<DeleteAnnouncementEvent>
{
    private readonly IDiscordService _discordService;

    public DeleteAnnouncementEventHandler(IDiscordService discordService)
    {
        _discordService = discordService;
    }

    public Task Handle(DeleteAnnouncementEvent request, CancellationToken cancellationToken)
    {
        return _discordService.DeleteMessage(request.Announcement.Message);
    }
}