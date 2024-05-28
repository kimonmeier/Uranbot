using UranBot.Public.Services;
using UranBot.Twitch.Plugin.Database;
using UranBot.Twitch.Plugin.Helper;

namespace UranBot.Twitch.Plugin.Events.ClipUpdate;

public class ClipUpdateEventHandler : IRequestHandler<ClipUpdateEvent>
{
    private readonly TwitchUranDbContext _dbContext;
    private readonly IDiscordService _discordService;

    public ClipUpdateEventHandler(TwitchUranDbContext dbContext, IDiscordService discordService)
    {
        _dbContext = dbContext;
        _discordService = discordService;
    }

    public async Task Handle(ClipUpdateEvent request, CancellationToken cancellationToken)
    { 
        EmbedBuilder embedBuilder = TwitchEmbedHelper.CreateClipEmbed(request.Clip);

        IMessage message = await _discordService.GetMessage(request.TwitchClip.DiscordMessage);
        EmbedBuilder? postedBuilder = message.Embeds.FirstOrDefault()?.ToEmbedBuilder();
        if (postedBuilder?.Equals(embedBuilder.Build().ToEmbedBuilder()) ?? false)
        {
            return;
        }
        
        await _discordService.UpdateMessage(request.TwitchClip.DiscordMessage, embedBuilder.Build());
    }
}