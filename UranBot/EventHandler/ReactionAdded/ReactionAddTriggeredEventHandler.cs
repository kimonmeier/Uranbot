using UranBot.Database;
using UranBot.Public.Events;

namespace UranBot.EventHandler.ReactionAdded;

public class ReactionAddTriggeredEventHandler : IRequestHandler<ReactionAddTriggeredEvent>
{
    private readonly CoreUranDbContext _dbContext;
    private readonly ISender _sender;
    private readonly IDiscordService _discordService;

    public ReactionAddTriggeredEventHandler(CoreUranDbContext dbContext, ISender sender, IDiscordService discordService)
    {
        _dbContext = dbContext;
        _sender = sender;
        _discordService = discordService;
    }

    public async Task Handle(ReactionAddTriggeredEvent request, CancellationToken cancellationToken)
    {
        IRequest<bool> requestToSend;
        if (request.Reaction.AddRequest is BaseReactionContextEvent baseReactionContext)
        {
            baseReactionContext.UserId = request.UserId;
            requestToSend = baseReactionContext;
        }
        else
        {
            requestToSend = request.Reaction.AddRequest;
        }
        
        var deleteMessage = await _sender.Send(requestToSend, cancellationToken);

        if (!deleteMessage)
        {
            return;
        }

        await _discordService.DeleteMessage(request.Reaction.MessageId);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}