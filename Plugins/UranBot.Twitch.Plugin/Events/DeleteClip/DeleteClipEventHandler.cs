using TwitchLib.Api;

namespace UranBot.Twitch.Plugin.Events.DeleteClip;

public class DeleteClipEventHandler : IRequestHandler<DeleteClipEvent>
{
    private readonly TwitchAPI _twitchApi;
    private readonly TwitchUranDbContext _dbContext;
    private readonly IDiscordService _discordService;

    public DeleteClipEventHandler(TwitchAPI twitchApi, TwitchUranDbContext dbContext, IDiscordService discordService)
    {
        _twitchApi = twitchApi;
        _dbContext = dbContext;
        _discordService = discordService;
    }

    public async Task Handle(DeleteClipEvent request, CancellationToken cancellationToken)
    {
        Clip? clip = (await _twitchApi.Helix.Clips.GetClipsAsync(new List<string>()
        {
            request.ClipUrl.Split('/').Last()
        })).Clips.SingleOrDefault();

        if (clip is null)
        {
            await request.SendFailureMessage("The provided clip couldn't be found by twitch itself");
            return;
        }

        TwitchClip? twitchClip = _dbContext.Set<TwitchClip>().SingleOrDefault(x => x.ClipId == clip.Id);

        if (twitchClip is null)
        {
            await request.SendFailureMessage("The provided clip couldn't be found on the discord");
            return;
        }

        await _discordService.DeleteMessage(twitchClip.DiscordMessage);
        _dbContext.Remove(twitchClip);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await request.SendSuccessMessage("The clip was deleted");
    }
}