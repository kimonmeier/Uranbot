using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Users.GetUsers;

namespace UranBot.Twitch.Plugin.Events.BroadcasterAdd;

public class BroadcasterAddEventHandler : IRequestHandler<BroadcasterAddEvent>
{
    private readonly TwitchUranDbContext _dbContext;
    private readonly TwitchAPI _twitchApi;

    public BroadcasterAddEventHandler(TwitchUranDbContext dbContext, TwitchAPI twitchApi)
    {
        _dbContext = dbContext;
        _twitchApi = twitchApi;
    }

    public async Task Handle(BroadcasterAddEvent request, CancellationToken cancellationToken)
    {
        long guildId = _dbContext.GetEntityIdByDiscordId<DiscordGuild>(request.InteractionContext.Guild.Id)!.Value;
        TwitchBroadcaster? twitchBroadcaster = _dbContext.Set<TwitchBroadcaster>().SingleOrDefault(x => x.GuildId == guildId && x.BroadcasterName == request.Name);

        if (twitchBroadcaster is not null)
        {
            await request.SendFailureMessage($"The broadcaster {request.Name} already exists");
            return;
        }

        GetUsersResponse usersResponse = await _twitchApi.Helix.Users.GetUsersAsync(null, new List<string>()
        {
            request.Name
        });

        User? user = usersResponse.Users.FirstOrDefault();
        if (user is null || user.DisplayName != request.Name)
        {
            await request.SendFailureMessage($"The broadcaster {request.Name} was not found by the twitch api");
            
            return;
        }

        _dbContext.Set<TwitchBroadcaster>().Add(new TwitchBroadcaster()
        {
            BroadcasterName = request.Name,
            GuildId = guildId,
            TwitchId = user.Id
        });
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        await request.SendSuccessMessage($"The broadcaster {request.Name} was added!");
    }
}