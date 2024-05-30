namespace UranBot.EventHandler.SyncGuildMember;

public class SyncGuildMemberEvent : IRequest<bool>
{
    public required SocketGuild Guild { get; set; }
    
    public required SocketUser User { get; set; }
}