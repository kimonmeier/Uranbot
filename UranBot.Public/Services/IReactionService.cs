namespace UranBot.Public.Services;

public interface IReactionService
{    
    Task<DiscordReaction> AddReaction(long discordMessageId, Emoji emote, IRequest<bool> eventOnAdd, IRequest<bool>? eventOnRemove = null);

    Task<DiscordReaction> AddReaction(long discordMessageId, string emoteName, IRequest<bool> eventOnAdd, IRequest<bool>? eventOnRemove = null);

    Task RemoveReaction(long discordReactionId);
}