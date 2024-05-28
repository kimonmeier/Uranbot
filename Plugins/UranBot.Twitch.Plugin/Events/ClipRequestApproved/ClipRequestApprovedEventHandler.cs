using UranBot.Twitch.Plugin.Events.PostClip;

namespace UranBot.Twitch.Plugin.Events.ClipRequestApproved;

public class ClipRequestApprovedEventHandler : IRequestHandler<ClipRequestApprovedEvent, bool>
{
    private readonly TwitchUranDbContext _dbContext;
    private readonly ISender _sender;

    public ClipRequestApprovedEventHandler(TwitchUranDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task<bool> Handle(ClipRequestApprovedEvent request, CancellationToken cancellationToken)
    {
        TwitchClipSettings clipSettings = _dbContext.Set<TwitchClipSettings>().Include(x => x.Broadcaster).Single(x => x.BroadcasterId == request.BroadcastId);

        await _sender.Send(new PostClipEvent()
        {
            Settings = clipSettings, Clip = request.Clip
        });

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}