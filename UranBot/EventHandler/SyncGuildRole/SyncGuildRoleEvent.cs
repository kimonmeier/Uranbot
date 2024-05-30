namespace UranBot.EventHandler.SyncGuildRole;

public class SyncGuildRoleEvent : IRequest
{
    public required SocketGuild Guild { get; set; }
    
    public required SocketRole Role { get; set; }
}