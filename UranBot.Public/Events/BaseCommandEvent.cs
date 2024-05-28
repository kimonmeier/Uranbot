namespace UranBot.Public.Events;

public abstract class BaseCommandEvent : IRequest
{
    public required IInteractionContext InteractionContext { get; init; }
}