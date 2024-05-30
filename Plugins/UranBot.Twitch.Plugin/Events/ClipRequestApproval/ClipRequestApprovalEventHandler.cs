using UranBot.Twitch.Plugin.Events.ClipRequestApproved;
using UranBot.Twitch.Plugin.Events.ClipRequestDenied;
using UranBot.Twitch.Plugin.Helper;

namespace UranBot.Twitch.Plugin.Events.ClipRequestApproval;

public class ClipRequestApprovalEventHandler : IRequestHandler<ClipRequestApprovalEvent>
{
    private readonly IReactionService _reactionService;
    private readonly IDiscordService _discordService;
    private readonly TwitchUranDbContext _dbContext;

    public ClipRequestApprovalEventHandler(IReactionService reactionService, TwitchUranDbContext dbContext, IDiscordService discordService)
    {
        _reactionService = reactionService;
        _dbContext = dbContext;
        _discordService = discordService;
    }

    public async Task Handle(ClipRequestApprovalEvent request, CancellationToken cancellationToken)
    {
        if (request.Settings.ApprovalChannelId is null)
        {
            throw new ArgumentException("No Approval Channel found");
        }

        long messageId = await _discordService.SendMessage(request.Settings.ApprovalChannelId.Value, TwitchEmbedHelper.CreateClipEmbed(request.Clip).Build());
        await _reactionService.AddReaction(messageId, Const.Discord.Emote.GreenCheck, new ClipRequestApprovedEvent()
        {
            BroadcastId = request.Settings.BroadcasterId, Clip = request.Clip
        });
        await _reactionService.AddReaction(messageId, Const.Discord.Emote.RedCross, new ClipRequestDeniedEvent());
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}