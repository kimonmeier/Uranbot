namespace UranBot.Public.Services;

public interface IDiscordService
{
    Task<long> SendMessage(long discordChannelId, string message);
    
    Task<long> SendMessage(long discordChannelId, Embed message);
    
    Task<long> SendMessage(long discordChannelId, string message, Embed embed);
    
    Task UpdateMessage(DiscordMessage message, Embed embed);
    
    Task UpdateMessage(DiscordMessage message, string messageContent);
    
    Task<IMessage?> GetMessage(long messageId);
    
    Task<IMessage?> GetMessage(DiscordMessage message);

    Task DeleteMessage(long discordMessageId);

    Task DeleteMessage(DiscordMessage discordMessage);
    
    Task AddRoleToUser(ulong guildId, ulong discordUserId, ulong discordRoleId);
    
    Task RemoveRoleFromUser(ulong guildId, ulong discordUserId, ulong discordRoleId);
}