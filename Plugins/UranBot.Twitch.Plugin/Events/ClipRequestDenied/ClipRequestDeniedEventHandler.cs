namespace UranBot.Twitch.Plugin.Events.ClipRequestDenied;

public class ClipRequestDeniedEventHandler : IRequestHandler<ClipRequestDeniedEvent, bool>
{
    private readonly TwitchUranDbContext _dbContext;

    public ClipRequestDeniedEventHandler(TwitchUranDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> Handle(ClipRequestDeniedEvent request, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}