using UranBot.Twitch.Plugin.Events.PostClip;

namespace UranBot.Twitch.Plugin.Events.PostClipManual;

public class PostClipManualEventHandler : IRequestHandler<PostClipManualEvent>
{
    private readonly ISender _sender;
    private readonly TwitchAPI _twitchApi;
    private readonly TwitchUranDbContext _dbContext;

    public PostClipManualEventHandler(TwitchUranDbContext dbContext, TwitchAPI twitchApi, ISender sender)
    {
        _dbContext = dbContext;
        _twitchApi = twitchApi;
        _sender = sender;
    }

    public async Task Handle(PostClipManualEvent request, CancellationToken cancellationToken)
    {
        Clip? clip = (await _twitchApi.Helix.Clips.GetClipsAsync(new List<string>()
        {
            request.ClipUrl.Split('/').Last()
        })).Clips.SingleOrDefault();

        if (clip is null)
        {
            await request.SendFailureMessage("The provided clip couldn't be found");
            return;
        }

        TwitchClipSettings? clipSettings = _dbContext.Set<TwitchClipSettings>().SingleOrDefault(x => x.Broadcaster.TwitchId == clip.BroadcasterId);

        if (clipSettings is null)
        {
            await request.SendFailureMessage($"The broadcaster {clip.BroadcasterName} doesn't have clip sharing enabled");
            return;
        }

        await _sender.Send(new PostClipEvent()
        {
            Clip = clip, Settings = clipSettings
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
        await request.SendSuccessMessage("The clip could be posted successfully");
    }
}