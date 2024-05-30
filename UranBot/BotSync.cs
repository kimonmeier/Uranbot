using Discord;

namespace UranBot;

public static class BotSync
{
    public async static Task SyncWithDatabase(this DiscordSocketClient client, DbContext dbContext)
    {
        var guilds = dbContext.Set<DiscordGuild>().ToList();

        foreach (SocketGuild socketGuild in client.Guilds)
        {
            DiscordGuild? guild = guilds.SingleOrDefault(x => x.DiscordId == socketGuild.Id);
            if (guild is not null)
            {
                guilds.Remove(guild);
            }
            
            socketGuild.SyncWithDatabase(dbContext);
            await socketGuild.SyncGuildMemberWithDatabase(dbContext);
            foreach (SocketTextChannel socketGuildChannel in socketGuild.Channels.Where(x => x.GetChannelType() == ChannelType.Text).Cast<SocketTextChannel>())
            {
                await socketGuildChannel.SyncWithDatabase(socketGuild, dbContext);
            }
        }

        dbContext.Set<DiscordGuild>().RemoveRange(guilds);
    }
    
    public static void SyncWithDatabase(this SocketGuild guild, DbContext dbContext)
    {
        DiscordGuild? discordGuild = dbContext.Set<DiscordGuild>().FirstOrDefault(x => x.DiscordId == guild.Id);
        if (discordGuild is null)
        {
            dbContext.Add(new DiscordGuild()
            {
                DiscordId = guild.Id
            });
        }
        else
        {
            // TODO: Update schreiben
        }

        dbContext.SaveChanges();
    }

    private static Task SyncWithDatabase(this SocketTextChannel channel, SocketGuild socketGuild, DbContext dbContext)
    {
        if (channel.GetChannelType() != ChannelType.Text)
        {
            return Task.CompletedTask;
        }
        
        DiscordChannel? discordChannel = dbContext.Set<DiscordChannel>().SingleOrDefault(x => x.DiscordId == channel.Id);
        
        if (discordChannel is null)
        {
            long guildId = dbContext.GetGuildIdByDiscordId(socketGuild.Id);
            discordChannel = new DiscordChannel()
            {
                DiscordId = channel.Id, GuildId = guildId, Name = channel.Name
            };
            
            dbContext.Add(discordChannel);
        }
        else
        {
            discordChannel.Name = channel.Name;
        }
        
        return Task.CompletedTask;
    }

    private static void SyncWithDatabase(this SocketUser user, DbContext dbContext)
    {
        if (!dbContext.Set<DiscordUser>().Any(x => x.DiscordId == user.Id))
        {
            dbContext.Add(new DiscordUser()
            {
                DiscordId = user.Id
            });
        }
        else
        {
            // TODO: Update schreiben
        }
    }

    public async static Task SyncGuildMemberWithDatabase(this SocketGuild guild, DbContext dbContext)
    {
        await guild.DownloadUsersAsync();

        foreach (SocketGuildUser guildUser in guild.Users)
        {
            if (guildUser.IsBot)
            {
                continue;
            }
            guildUser.SyncWithDatabase(dbContext);
        }

        List<DiscordGuildMember> guildMembers = dbContext.Set<DiscordGuildMember>().Where(x => x.GuildId == dbContext.GetGuildIdByDiscordId(guild.Id)).ToList();
        
        foreach (SocketGuildUser user in guild.Users)
        {
            if (user.IsBot)
            {
                continue;
            }
            long userId = dbContext.GetUserIdByDiscordId(user.Id);
            long guildId = dbContext.GetGuildIdByDiscordId(guild.Id);
                
            if (!guildMembers.Any(x => x.UserId == userId && x.GuildId == guildId))
            {
                dbContext.Add(new DiscordGuildMember()
                {
                    GuildId = guildId, UserId = userId
                });
            }
            else
            {
                guildMembers.RemoveAll(x => x.UserId == userId && x.GuildId == guildId);
            }
        }

        dbContext.Set<DiscordGuildMember>().RemoveRange(guildMembers);
    }
}