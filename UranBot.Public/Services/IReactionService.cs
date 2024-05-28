namespace UranBot.Public.Services;

public interface IReactionService
{
    Task<DiscordReaction> AddReaction(long discordMessageId, string emoteName, IRequest<bool> @event);

    Task RemoveReaction(long discordReactionId);
}