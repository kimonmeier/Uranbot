using UranBot.Database;

namespace UranBot.EventHandler.ReactionAdded;

public class ReactionTriggeredEventHandler : IRequestHandler<ReactionTriggeredEvent>
{
    private readonly CoreUranDbContext _dbContext;
    private readonly ISender _sender;
    private readonly IDiscordService _discordService;

    public ReactionTriggeredEventHandler(CoreUranDbContext dbContext, ISender sender, IDiscordService discordService)
    {
        _dbContext = dbContext;
        _sender = sender;
        _discordService = discordService;
    }

    public async Task Handle(ReactionTriggeredEvent request, CancellationToken cancellationToken)
    {
        var deleteMessage = await _sender.Send(request.Reaction.Request, cancellationToken);

        if (!deleteMessage)
        {
            return;
        }

        await _discordService.DeleteMessage(request.Reaction.MessageId);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}