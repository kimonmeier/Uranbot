namespace UranBot.Public.Events;

public abstract class BaseReactionContextEvent : IRequest<bool>
{
    public ulong UserId { get; set; }
}