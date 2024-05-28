using UranBot.Public.Events;

namespace UranBot.Plugins;

public sealed class UranInteractionService : IUranInteractionService
{
    private readonly IServiceProvider _serviceProvider;

    public UranInteractionService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ExecuteInteraction<TEvent>(TEvent @event) where TEvent : BaseCommandEvent
    {
        using IServiceScope serviceScope = _serviceProvider.CreateScope();

        await serviceScope.ServiceProvider.GetRequiredService<ISender>().Send(@event);
    }
}