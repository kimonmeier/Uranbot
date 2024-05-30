using Discord;
using UranBot.Twitch.Plugin.Helper;

namespace UranBot.Twitch.Plugin.Events.PostClip;

public class PostClipEventHandler : IRequestHandler<PostClipEvent>
{
    private readonly TwitchUranDbContext _dbContext;
    private readonly IDiscordService _discordService;

    public PostClipEventHandler(TwitchUranDbContext dbContext, IDiscordService discordService)
    {
        _dbContext = dbContext;
        _discordService = discordService;
    }

    public async Task Handle(PostClipEvent request, CancellationToken cancellationToken)
    {
        EmbedBuilder embedBuilder = TwitchEmbedHelper.CreateClipEmbed(request.Clip);
        
        long discordMessageId = await _discordService.SendMessage(request.Settings.DiscordChannelId, embedBuilder.Build());
        
        _dbContext.Set<TwitchClip>().Add(new TwitchClip()
        {
            BroadcasterId = request.Settings.Broadcaster.Id, DiscordMessageId = discordMessageId, ClipId = request.Clip.Id, PostedAt = DateTime.Now
        });
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}