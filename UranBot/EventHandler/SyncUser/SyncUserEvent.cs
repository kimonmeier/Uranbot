namespace UranBot.EventHandler.SyncUser;

public class SyncUserEvent : IRequest
{
    public required SocketUser User { get; set; }
    
}