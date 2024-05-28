using UranBot.Public.Events;

namespace UranBot.Public.Services;

public interface IUranInteractionService
{
    Task ExecuteInteraction<TEvent>(TEvent @event) where TEvent : BaseCommandEvent;
}