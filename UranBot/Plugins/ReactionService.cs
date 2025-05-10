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

    public async Task<DiscordReaction> AddReaction(long discordMessageId, string emoteName, IRequest<bool> eventOnAdd, IRequest<bool>? eventOnRemove = null)
    {
        DiscordMessage discordMessage = _dbContext.Set<DiscordMessage>().Single(x => x.Id == discordMessageId);
        SocketChannel socketChannel = _discordSocketClient.GetChannel(_dbContext.Set<DiscordChannel>().Single(x => x.Id == discordMessage.ChannelId).DiscordId);
        if (socketChannel is not SocketTextChannel textChannel)
        {
            throw new ArgumentException("The provided channel was not a text channel");
        }


        IMessage message = await textChannel.GetMessageAsync(discordMessage.DiscordId);
        Emoji emote = Emoji.Parse(emoteName);
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