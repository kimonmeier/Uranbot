using UranBot.Database;
using UranBot.Public.Events;

namespace UranBot.EventHandler.ReactionRemoved;

public class ReactionRemoveTriggeredEventHandler : IRequestHandler<ReactionRemoveTriggeredEvent>
{
    private readonly CoreUranDbContext _dbContext;
    private readonly ISender _sender;
    private readonly IDiscordService _discordService;

    public ReactionRemoveTriggeredEventHandler(CoreUranDbContext dbContext, ISender sender, IDiscordService discordService)
    {
        _dbContext = dbContext;
        _sender = sender;
        _discordService = discordService;
    }

    public async Task Handle(ReactionRemoveTriggeredEvent request, CancellationToken cancellationToken)
    {
        if (request.Reaction.RemoveRequest is null)
        {
            return;
        }
        
        IRequest<bool> requestToSend;
        if (request.Reaction.RemoveRequest is BaseReactionContextEvent baseReactionContext)
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