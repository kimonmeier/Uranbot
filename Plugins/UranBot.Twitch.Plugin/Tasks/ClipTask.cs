using Microsoft.Extensions.Logging;
using TwitchLib.Api;
using UranBot.Twitch.Plugin.Events.ClipRequestApproval;
using UranBot.Twitch.Plugin.Events.ClipUpdate;
using UranBot.Twitch.Plugin.Events.PostClip;

namespace UranBot.Twitch.Plugin.Tasks;

public class ClipTask
{
    private readonly TwitchAPI _twitchApi;
    private readonly TwitchUranDbContext _dbContext;
    private readonly ILogger<ClipTask> _logger;
    private readonly ISender _sender;

    public ClipTask(TwitchAPI twitchApi, TwitchUranDbContext dbContext, ILogger<ClipTask> logger, ISender sender)
    {
        _twitchApi = twitchApi;
        _dbContext = dbContext;
        _logger = logger;
        _sender = sender;
    }

    public async Task CheckClips()
    {
        _logger.LogInformation("Checking for Clips");
        foreach (TwitchBroadcaster twitchBroadcaster in _dbContext.Set<TwitchBroadcaster>())
        {
            await CheckNewClipsForBroadcaster(twitchBroadcaster);
            await UpdateClipsForBroadcaster(twitchBroadcaster);
        }

        await _dbContext.SaveChangesAsync();
    }

    private async Task CheckNewClipsForBroadcaster(TwitchBroadcaster broadcaster)
    {
        TwitchClipSettings? twitchClipSettings = _dbContext.Set<TwitchClipSettings>().SingleOrDefault(x => x.BroadcasterId == broadcaster.Id);
        if (twitchClipSettings is null)
        {
            _logger.LogInformation("The broadcaster {0} has no clip channel set", broadcaster.BroadcasterName);

            return;
        }

        if (twitchClipSettings.ShareMode == TwitchClipShareMode.Manual)
        {
            return;
        }

        GetClipsResponse clipsResponse = await _twitchApi.Helix.Clips.GetClipsAsync(broadcasterId: broadcaster.TwitchId, startedAt: twitchClipSettings.LastSynched.AddMinutes(-1), endedAt: DateTime.Now.AddDays(1), first: 100);

        twitchClipSettings.LastSynched = DateTime.Now;

        foreach (Clip clip in clipsResponse.Clips)
        {
            await HandleClip(clip, twitchClipSettings);
        }
    }

    private async Task HandleClip(Clip clip, TwitchClipSettings settings)
    {
        _logger.LogDebug("Found new Clip \"{0}\" for {1}", clip.Title, clip.BroadcasterName);

        if (settings.ShareMode == TwitchClipShareMode.Automatic)
        {
            await _sender.Send(new PostClipEvent()
            {
                Settings = settings, Clip = clip
            });
        }
        else
        {
            await _sender.Send(new ClipRequestApprovalEvent()
            {
                Settings = settings, Clip = clip
            });
        }
    }

    private async Task UpdateClipsForBroadcaster(TwitchBroadcaster broadcaster)
    {
        var twitchClips = _dbContext.Set<TwitchClip>().Where(x => x.BroadcasterId == broadcaster.Id).OrderBy(x => x.PostedAt).Take(100).ToList();

        var twitchClipIds = twitchClips.Select(x => x.ClipId).ToList();

        if (!twitchClipIds.Any())
        {
            return;
        }

        GetClipsResponse clipsResponse = await _twitchApi.Helix.Clips.GetClipsAsync(twitchClipIds);

        foreach (Clip clip in clipsResponse.Clips)
        {
            _logger.LogDebug("Update Clip \"{0}\" for {1}", clip.Title, clip.BroadcasterName);
            await _sender.Send(new ClipUpdateEvent()
            {
                TwitchClip = twitchClips.Single(x => x.ClipId == clip.Id), Clip = clip
            });
        }
    }
}