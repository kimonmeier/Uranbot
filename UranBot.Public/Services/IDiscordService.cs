namespace UranBot.Public.Services;

public interface IDiscordService
{
    Task<long> SendMessage(long discordChannelId, string message);
    
    Task<long> SendMessage(long discordChannelId, Embed message);
    
    Task UpdateMessage(DiscordMessage message, Embed embed);
    
    Task<IMessage> GetMessage(long messageId);
    
    Task<IMessage> GetMessage(DiscordMessage message);

    Task DeleteMessage(long discordMessageId);

    Task DeleteMessage(DiscordMessage discordMessage);
}