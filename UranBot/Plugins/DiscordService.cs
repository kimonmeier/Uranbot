using Discord;
using Discord.Rest;
using UranBot.Database;

namespace UranBot.Plugins;

public class DiscordService : IDiscordService
{
    private readonly DiscordSocketClient _discordSocketClient;
    private readonly CoreUranDbContext _dbContext;

    public DiscordService(DiscordSocketClient discordSocketClient, CoreUranDbContext dbContext)
    {
        _discordSocketClient = discordSocketClient;
        _dbContext = dbContext;
    }

    public Task<long> SendMessage(long discordChannelId, string message)
    {
        return PrivateSendMessage(discordChannelId, message);
    }

    public Task<long> SendMessage(long discordChannelId, Embed message)
    {
        return PrivateSendMessage(discordChannelId, embed: message);
    }

    public Task<long> SendMessage(long discordChannelId, string message, Embed embed)
    {
        return PrivateSendMessage(discordChannelId, message, embed);
    }

    public Task UpdateMessage(DiscordMessage message, Embed embed)
    {
        SocketTextChannel? socketTextChannel = _discordSocketClient.GetChannel(message.Channel.DiscordId) as SocketTextChannel;
        if (socketTextChannel is null)
        {
            throw new ArgumentException($"The Provided Discord Channel is not a TextChannel");
        }
        
        return socketTextChannel.ModifyMessageAsync(message.DiscordId, x => x.Embed = embed);
    }

    public Task UpdateMessage(DiscordMessage message, string messageContent)
    {
        SocketTextChannel? socketTextChannel = _discordSocketClient.GetChannel(message.Channel.DiscordId) as SocketTextChannel;
        if (socketTextChannel is null)
        {
            throw new ArgumentException($"The Provided Discord Channel is not a TextChannel");
        }
        
        return socketTextChannel.ModifyMessageAsync(message.DiscordId, x => x.Content = messageContent);
    }

    public Task<IMessage> GetMessage(long messageId)
    {
        return GetMessage(_dbContext.Set<DiscordMessage>().Single(x => x.Id == messageId));
    }

    public Task<IMessage> GetMessage(DiscordMessage message)
    {
        return (_discordSocketClient.GetChannel(message.Channel.DiscordId) as SocketTextChannel)!.GetMessageAsync(message.DiscordId);
    }

    private async Task<long> PrivateSendMessage(long discordChannelId, string? message = null, Embed? embed = null)
    {
        var channelInfos = await _dbContext.Set<DiscordChannel>().Include(x => x.Guild).Where(x => x.Id == discordChannelId).Select(x => new { ChannelId = x.DiscordId, GuildId = x.Guild.DiscordId}).FirstOrDefaultAsync();

        if (channelInfos is null)
        {
            throw new ArgumentNullException($"No Discord Channel with Id {discordChannelId} was found in the Database");
        }

        SocketChannel socketChannel = _discordSocketClient.GetGuild(channelInfos.GuildId).GetChannel(channelInfos.ChannelId);

        if (socketChannel is not SocketTextChannel textChannel)
        {
            throw new ArgumentException($"The Provided Discord Channel is not a TextChannel");
        }
        RestUserMessage restUserMessage = await textChannel.SendMessageAsync(message, embed: embed);

        DiscordMessage discordMessage = new()
        {
            DiscordId = restUserMessage.Id, ChannelId = discordChannelId, IsDeleted = false
        };

        _dbContext.Set<DiscordMessage>().Add(discordMessage);
        await _dbContext.SaveChangesAsync();

        return _dbContext.Set<DiscordMessage>().Single(x => x.DiscordId == restUserMessage.Id).Id;
    }

    public Task DeleteMessage(long discordMessageId)
    {
        return DeleteMessage(_dbContext.Set<DiscordMessage>().Include(x => x.Channel).Single(x => x.Id == discordMessageId));
    }

    public async Task DeleteMessage(DiscordMessage discordMessage)
    {
        SocketChannel channel = _discordSocketClient.GetChannel(discordMessage.Channel.DiscordId);
        if (channel is not SocketTextChannel textChannel)
        {
            return;
        }

        IMessage message = await textChannel.GetMessageAsync(discordMessage.DiscordId);
        await message.DeleteAsync();

        _dbContext.Set<DiscordMessage>().Remove(_dbContext.Set<DiscordMessage>().Single(x => x.Id == discordMessage.Id));
    }
}