namespace UranBot.EventHandler.SyncGuildChannel;

public class SyncGuildChannelEvent : IRequest
{
    public required SocketGuild Guild { get; init; }
    
    public required SocketTextChannel Channel { get; init; }
}