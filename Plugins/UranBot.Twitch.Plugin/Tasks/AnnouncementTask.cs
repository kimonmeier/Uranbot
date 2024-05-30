using TwitchLib.Api.Helix.Models.Channels.GetChannelInformation;
using TwitchLib.Api.Helix.Models.Streams.GetStreams;
using UranBot.Twitch.Plugin.Events.DeleteAnnouncement;
using UranBot.Twitch.Plugin.Events.PostAnnouncement;
using UranBot.Twitch.Plugin.Events.UpdateAnnouncement;
using Stream = TwitchLib.Api.Helix.Models.Streams.GetStreams.Stream;

namespace UranBot.Twitch.Plugin.Tasks;

public class AnnouncementTask
{
    private readonly TwitchAPI _twitchApi;
    private readonly TwitchUranDbContext _dbContext;
    private readonly ISender _sender;
    private readonly Dictionary<long, DateTime> _searchedBroadcasters;

    public AnnouncementTask(TwitchAPI twitchApi, TwitchUranDbContext dbContext, ISender sender)
    {
        _twitchApi = twitchApi;
        _dbContext = dbContext;
        _sender = sender;
        _searchedBroadcasters = new Dictionary<long, DateTime>();
    }

    public async Task CheckBroadcaster()
    {
        foreach (TwitchBroadcaster broadcaster in _dbContext.Set<TwitchBroadcaster>())
        {
            _searchedBroadcasters.TryAdd(broadcaster.Id, DateTime.MinValue);
            await CheckBroadcaster(broadcaster);
        }
    }

    private async Task CheckBroadcaster(TwitchBroadcaster broadcaster)
    {
        GetStreamsResponse streamInfos = await _twitchApi.Helix.Streams.GetStreamsAsync(userIds: new List<string>() { broadcaster.TwitchId });

        if (!streamInfos.Streams.Any())
        {
            await CheckForOfflineBroadcaster(broadcaster);
        }
        else
        {
            await CheckForOnlineBroadcaster(broadcaster, streamInfos.Streams.Single());
        }
    }

    private async Task CheckForOfflineBroadcaster(TwitchBroadcaster broadcaster)
    {
        TwitchAnnouncement? announcement = _dbContext.Set<TwitchAnnouncement>().SingleOrDefault(x => x.BroadcasterId == broadcaster.Id);
        if (announcement is null)
        {
            return;
        }

        await _sender.Send(new DeleteAnnouncementEvent()
        {
            Announcement = announcement
        });
    }

    private async Task CheckForOnlineBroadcaster(TwitchBroadcaster broadcaster, Stream stream)
    {
        TwitchAnnouncement? announcement = _dbContext.Set<TwitchAnnouncement>().SingleOrDefault(x => x.BroadcasterId == broadcaster.Id);
        if (announcement is not null)
        {
            await _sender.Send(new UpdateAnnouncementEvent()
            {
                Announcement = announcement, Stream = stream
            });
            return;
        }

        // When user was online in the last 5 minutes he probably had a disconnect, so we don't have to announce him a second time
        if (_searchedBroadcasters[broadcaster.Id] > DateTime.Now.AddMinutes(-10))
        {
            return;
        }

        await _sender.Send(new PostAnnouncementEvent()
        {
            Broadcaster = broadcaster, Stream = stream
        });
    }
}