using Discord;
using UranBot.Database;

namespace UranBot.Plugins;

public class ReactionService : IReactionService
{
    private readonly CoreUranDbContext _dbContext;
    private readonly DiscordSocketClient _discordSocketClient;

    public ReactionService(DiscordSocketClient discordSocketClient, CoreUranDbContext dbContext)
    {
        _discordSocketClient = discordSocketClient;
        _dbContext = dbContext;
    }

    public Task<DiscordReaction> AddReaction(long discordMessageId, string emoteName, IRequest<bool> eventOnAdd, IRequest<bool>? eventOnRemove = null)
    {
        return AddReaction(discordMessageId, Emoji.Parse(emoteName), eventOnAdd, eventOnRemove);
    }

    public async Task<DiscordReaction> AddReaction(ulong channelId, ulong discordMessageId, Emoji emote, IRequest<bool> eventOnAdd, IRequest<bool>? eventOnRemove = null)
    {
        var dbChannel = _dbContext.Set<DiscordChannel>().Include(x => x.Guild).SingleOrDefault(x => x.DiscordId == channelId);

        if (dbChannel is null)
        {
            throw new ArgumentException("The provided channel was not found in the database");
        }

        var discordMessage = await _discordSocketClient.GetGuild(dbChannel.Guild.DiscordId).GetTextChannel(dbChannel.DiscordId).GetMessageAsync(discordMessageId);
        await discordMessage.AddReactionAsync(emote);
        
        DiscordMessage message = new DiscordMessage()
        {
            ChannelId = dbChannel.Id, DiscordId = discordMessageId, IsDeleted = false, UserId = _dbContext.Set<DiscordUser>().Single(x => x.DiscordId == discordMessage.Author.Id).Id
        };

        DiscordReaction reaction = new()
        {
            Message = message, AddRequest = eventOnAdd, RemoveRequest = eventOnRemove, EmoteName = emote.Name,
        };
        
        await _dbContext.Set<DiscordReaction>().AddAsync(reaction);
        await _dbContext.Set<DiscordMessage>().AddAsync(message);
        await _dbContext.SaveChangesAsync();
        return reaction;
    }

    public async Task<DiscordReaction> AddReaction(long discordMessageId, Emoji emote, IRequest<bool> eventOnAdd, IRequest<bool>? eventOnRemove = null)
    {
        DiscordMessage discordMessage = _dbContext.Set<DiscordMessage>().Single(x => x.Id == discordMessageId);
        SocketChannel socketChannel = _discordSocketClient.GetChannel(_dbContext.Set<DiscordChannel>().Single(x => x.Id == discordMessage.ChannelId).DiscordId);
        if (socketChannel is not SocketTextChannel textChannel)
        {
            throw new ArgumentException("The provided channel was not a text channel");
        }


        IMessage message = await textChannel.GetMessageAsync(discordMessage.DiscordId);
        await message.AddReactionAsync(emote);

        DiscordReaction reaction = new()
        {
            MessageId = discordMessageId, AddRequest = eventOnAdd, RemoveRequest = eventOnRemove, EmoteName = emote.Name,
        };
        

        _dbContext.Set<DiscordReaction>().Add(reaction);

        await _dbContext.SaveChangesAsync();
        return reaction;
    }

    public async Task RemoveReaction(long discordReactionId)
    {
        _dbContext.Remove(_dbContext.Set<DiscordReaction>().Single(x => x.Id == discordReactionId));
        await _dbContext.SaveChangesAsync();
    }
}