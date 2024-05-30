namespace UranBot.EventHandler.SyncGuild;

public class SyncGuildEvent : IRequest<DiscordGuild>
{
    public required SocketGuild SocketGuild { get; set; }
}